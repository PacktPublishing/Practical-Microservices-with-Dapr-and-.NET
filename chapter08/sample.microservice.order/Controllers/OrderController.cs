using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapr;
using Dapr.Client;
using Dapr.Client.Http;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.order;
using sample.microservice.dto.reservation;
using sample.microservice.state.order;
using sample.microservice.dto.customization;
using System.Linq;

namespace sample.microservice.order.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        public const string StoreName = "orderstore";
        public const string PubSub = "commonpubsub";

        /// <summary>
        /// Method for submitting a new order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("order")]
        public async Task<ActionResult<Guid>> SubmitOrder(Order order, [FromServices] DaprClient daprClient)
        {
            if (!Validate(order)) { return BadRequest(); }
            // order validated
            order.Id = Guid.NewGuid();

            var state = await daprClient.GetStateEntryAsync<OrderState>(StoreName, order.Id.ToString());
            state.Value ??= new OrderState() { CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Order = order };

            var options = new StateOptions() {Concurrency = ConcurrencyMode.FirstWrite, Consistency = ConsistencyMode.Strong};
            await state.SaveAsync(stateOptions: options);

            await daprClient.PublishEventAsync<Order>(PubSub, common.Topics.OrderSubmittedTopicName, order);

            Console.WriteLine($"Submitted order {order.Id}");
            return order.Id;
        }

        /// <summary>
        /// Method for submitting a new order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, common.Topics.ReservationFailedTopicName)]
        [HttpPost(common.Topics.ReservationFailedTopicName)]
        public async Task<ActionResult<Guid>> OnReservationFailed(OrderReservation reservation, [FromServices] DaprClient daprClient)
        {             
            var state = await daprClient.GetStateEntryAsync<OrderState>(StoreName, reservation.OrderId.ToString());
            if (state.Value == null)
            {
                return this.NotFound();
            }

            state.Value.Status = "reservation failed";
            state.Value.UpdatedOn = DateTime.UtcNow;

            await state.SaveAsync();

            Console.WriteLine($"Acknowledged reservation failed for order {reservation.OrderId}");
            return this.Ok();
        }

        /// <summary>
        /// Method for submitting a new order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, common.Topics.CustomizationFailedTopicName)]
        [HttpPost(common.Topics.CustomizationFailedTopicName)]
        public async Task<ActionResult<Guid>> OnCustomizationFailed(OrderCustomization customization, [FromServices] DaprClient daprClient)
        {             
            var state = await daprClient.GetStateEntryAsync<OrderState>(StoreName, customization.OrderId.ToString());
            if (state.Value == null)
            {
                return this.NotFound();
            }

            state.Value.Status = "customization failed";
            state.Value.UpdatedOn = DateTime.UtcNow;

            await state.SaveAsync();

            Console.WriteLine($"Acknowledged customization failed for order {customization.OrderId}");
            return this.Ok();
        }

        /// <summary>
        /// Method for retrieving an order.
        /// </summary>
        /// <param name="orderid">Order Id state info.</param>
        /// <returns>Order information</returns>
        [HttpGet("order/{state}")]
        public ActionResult<Order> Get([FromState(StoreName)]StateEntry<OrderState> state)
        {           
            if (state.Value == null)
            {
                return this.NotFound();
            }
            var result = state.Value.Order;

            Console.WriteLine($"Retrieved order {result.Id} ");

            return result;
        }

        private static bool Validate(Order order)
        {
            // validation
            var groupedItem = 
                from item in order.Items
                group item by item.ProductCode into items
                select new 
                {
                    ProductCode = items.Key,
                    Quantity = items.Sum(x => x.Quantity)
                };            
            var groupedSpecial = 
                from item in order.SpecialRequests
                group item by item.Scope.ProductCode into items
                select new 
                {
                    ProductCode = items.Key,
                    Quantity = items.Sum(x => x.Scope.Quantity)
                };
            //special requests should be contained in ordered items
            foreach (var itemSpecial in groupedSpecial)
            {
                var itemOrder = groupedItem.Where(e => e.ProductCode == itemSpecial.ProductCode).FirstOrDefault();
                if ( itemOrder == null || itemOrder.Quantity < itemSpecial.Quantity )
                {
                    return false;
                }
            }
            return true;
        }
    }
}
