using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.order;
using sample.microservice.dto.reservation;
using sample.microservice.state.reservation;
using sample.microservice.dto.customization;
using System.Linq;
using sample.microservice.reservationactor.interfaces;
using Dapr.Actors;
using Dapr.Actors.Client;

namespace sample.microservice.reservation.Controllers
{
    [ApiController]
    public class ReservationController : ControllerBase
    {
        public const string StoreName_reservation = "reservationstore";
        public const string PubSub = "commonpubsub";
        
        /// <summary>
        /// Method for reserving all items quantity in an order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, common.Topics.OrderSubmittedTopicName)]
        [HttpPost(common.Topics.OrderSubmittedTopicName)]
        public async Task<ActionResult<OrderReservation>> ReserveOrder(Order order, [FromServices] DaprClient daprClient)
        {            
            // retrieve state of whole reservation request
            var stateReservation = await daprClient.GetStateEntryAsync<ReservationState>(StoreName_reservation, order.Id.ToString());
            stateReservation.Value ??= new ReservationState() { OrderId = order.Id, ReservedItems = new List<ItemReservation>() };

            var result = new OrderReservation(){ OrderId = order.Id, ReservedItems = new List<Item>()};
            // iterate over state of each reservation item
            foreach (var item in order.Items)
            {
                var SKU = item.ProductCode;
                var quantity = item.Quantity;

                var actorID = new ActorId(SKU);
                var proxy = ActorProxy.Create<IReservationItemActor>(actorID,"ReservationItemActor");
                var balanceQuantity = await proxy.AddReservation(quantity);

                // compose result
                result.ReservedItems.Add(new Item{SKU = SKU, BalanceQuantity = balanceQuantity});

                // save updated reservation state
                stateReservation.Value.ReservedItems.Add(new ItemReservation() { SKU = SKU, Quantity = quantity, ReservedOn = DateTime.UtcNow });
                await stateReservation.SaveAsync();

                Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} of {SKU} for {quantity}, balance {balanceQuantity}");
            }
        
            await daprClient.PublishEventAsync<Order>(PubSub,common.Topics.ReservationCompletedTopicName, order);

            Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} completed");
            return this.Ok();
        }

        /// <summary>
        /// Method for reserving all items quantity in an order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, common.Topics.CustomizationFailedTopicName)]
        [HttpPost(common.Topics.CustomizationFailedTopicName)]
        public async Task<ActionResult<Guid>> OnCustomizationFailed(OrderCustomization customization, [FromServices] DaprClient daprClient)
        {            
            // retrieve state of whole reservation request
            var stateReservation = await daprClient.GetStateEntryAsync<ReservationState>(StoreName_reservation, customization.OrderId.ToString());
            if (stateReservation.Value == null)
            {
                return this.NotFound();
            }

            // ReservedItems: group by SKU & sum by Quantity 
            var groupedReserved = 
                from item in stateReservation.Value.ReservedItems
                group item by item.SKU into items
                select new 
                {
                    SKU = items.Key,
                    Quantity = items.Sum(x => x.Quantity)
                };

            // CustomizedItems: group by SKU & sum by Quantity 
            var groupedCustomized = 
                from item in customization.CustomizedItems
                group item by item.SKU into items
                select new 
                {
                    SKU = items.Key,
                    Quantity = items.Sum(x => x.Quantity)
                };

            foreach (var itemReserved in groupedReserved)
            {
                var customizedItem = groupedCustomized.Where(e => e.SKU == itemReserved.SKU).FirstOrDefault();
                var compensateQuantity = (itemReserved.Quantity - (customizedItem == null ? 0 : customizedItem.Quantity));
                if (compensateQuantity > 0)
                {
                    var SKU = itemReserved.SKU;
                    // compensate balance
                    var quantity = -compensateQuantity;

                    var actorID = new ActorId(SKU);
                    var proxy = ActorProxy.Create<IReservationItemActor>(actorID,"ReservationItemActor");
                    var balanceQuantity = await proxy.AddReservation(quantity);

                    // save updated reservation state
                    stateReservation.Value.ReservedItems.Add(new ItemReservation() { SKU = SKU, Quantity = quantity, ReservedOn = DateTime.UtcNow });
                    await stateReservation.SaveAsync();

                    Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} of {SKU} for {quantity}, balance {balanceQuantity}");
                }
            }
        
            Console.WriteLine($"Acknowledged customization failed for order {customization.OrderId}");
            return this.Ok();
        }

        /// <summary>
        /// Method for reserving an item quantity.
        /// </summary>
        /// <param name="reservation">Reservation info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("balance/{SKU}")]
        public async Task<ActionResult<Item>> Get(string SKU, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter item retrieval");
            
            if (string.IsNullOrEmpty(SKU))
            {
                return this.NotFound();
            }
            var actorID = new ActorId(SKU);
            var proxy = ActorProxy.Create<IReservationItemActor>(actorID,"ReservationItemActor");
            var balanceQuantity = await proxy.GetBalance();

            var result = new Item() {SKU = SKU, BalanceQuantity= balanceQuantity};

            Console.WriteLine($"Retrieved {result.SKU} is {result.BalanceQuantity}");

            return result;
        }
    }
}
