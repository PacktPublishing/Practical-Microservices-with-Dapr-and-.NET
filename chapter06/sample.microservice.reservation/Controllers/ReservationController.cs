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

namespace sample.microservice.reservation.Controllers
{
    [ApiController]
    public class ReservationController : ControllerBase
    {
        public const string StoreName_reservation = "reservationstore";
        public const string StoreName_item = "reservationitemstore";
        public const string PubSub = "commonpubsub";
        
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

                var stateItem = await daprClient.GetStateEntryAsync<ItemState>(StoreName_item, SKU);
                stateItem.Value ??= new ItemState() { SKU = SKU, Changes = new List<ItemReservation>() };

                // update balance
                stateItem.Value.BalanceQuantity -= quantity;

                // record change
                ItemReservation change = new ItemReservation() { SKU = SKU, Quantity = quantity, ReservedOn = DateTime.UtcNow };
                stateItem.Value.Changes.Add(change);
                // keep only the 10 latest changes
                if (stateItem.Value.Changes.Count > 10) stateItem.Value.Changes.RemoveAt(0);
                
                // save item state
                await stateItem.SaveAsync();

                // compose result
                result.ReservedItems.Add(new Item{SKU = SKU, BalanceQuantity = stateItem.Value.BalanceQuantity});

                // save updated reservation state
                stateReservation.Value.ReservedItems.Add(change);
                await stateReservation.SaveAsync();

                Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} of {SKU} for {quantity}, balance {stateItem.Value.BalanceQuantity}");
            }
        
            await daprClient.PublishEventAsync<Order>(PubSub,common.Topics.ReservationCompletedTopicName, order);

            Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} completed");
            return this.Ok();
        }

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

                    // retrieve reservation item from state
                    var stateItem = await daprClient.GetStateEntryAsync<ItemState>(StoreName_item, SKU);

                    // update balance
                    stateItem.Value.BalanceQuantity -= quantity;

                    // record change
                    ItemReservation change = new ItemReservation() { SKU = SKU, Quantity = quantity, ReservedOn = DateTime.UtcNow };
                    stateItem.Value.Changes.Add(change);
                    if (stateItem.Value.Changes.Count > 10) stateItem.Value.Changes.RemoveAt(0);

                    // save item state
                    await stateItem.SaveAsync();

                    // save updated reservation state
                    stateReservation.Value.ReservedItems.Add(change);
                    await stateReservation.SaveAsync();

                    Console.WriteLine($"Reservation in {stateReservation.Value.OrderId} of {SKU} for {quantity}, balance {stateItem.Value.BalanceQuantity}");
                }
            }
        
            Console.WriteLine($"Acknowledged customization failed for order {customization.OrderId}");
            return this.Ok();
        }

        [HttpGet("balance/{state}")]
        public ActionResult<Item> Get([FromState(StoreName_item)]StateEntry<ItemState> state, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter item retrieval");
            
            if (state.Value == null)
            {
                return this.NotFound();
            }
            var result = new Item() {SKU = state.Value.SKU, BalanceQuantity= state.Value.BalanceQuantity};

            Console.WriteLine($"Retrieved {result.SKU} is {result.BalanceQuantity}");

            return result;
        }
    }
}