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

namespace sample.microservice.order.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        public const string StoreName = "orderstore";

        /// <summary>
        /// Method for submitting a new order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("order")]
        public async Task<ActionResult<Guid>> SubmitOrder(Order order, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter submit order");
            
            order.Id = Guid.NewGuid();

            var state = await daprClient.GetStateEntryAsync<OrderState>(StoreName, order.Id.ToString());
            state.Value ??= new OrderState() { CreatedOn = DateTime.UtcNow, UpdatedOn = DateTime.UtcNow, Order = order };
            
            HTTPExtension httpExtension = new HTTPExtension()
            {
                Verb = HTTPVerb.Post
            };
            foreach (var item in order.Items)
            {
                var data = new Item() { SKU = item.ProductCode, Quantity = item.Quantity };
                var result = await daprClient.InvokeMethodAsync<Item, Item>("reservation-service", "reserve", data, httpExtension);
            }
            
            // Console.WriteLine($"ETag {state.ETag}");
            // var options = new StateOptions() {Concurrency = ConcurrencyMode.FirstWrite, Consistency = ConsistencyMode.Strong};
            // await state.SaveAsync(options);
            // var metadata = new Dictionary<string,string>();
            // metadata.Add("partitionKey","something_else");
            // await state.SaveAsync(metadata: metadata);
            await state.SaveAsync();

            Console.WriteLine($"Submitted order {order.Id}");

            return order.Id;
        }

        /// <summary>
        /// Method for retrieving an order.
        /// </summary>
        /// <param name="orderid">Order Id state info.</param>
        /// <returns>Order information</returns>
        [HttpGet("order/{state}")]
        public ActionResult<Order> Get([FromState(StoreName)]StateEntry<OrderState> state)
        {
            Console.WriteLine("Enter order retrieval");
            
            if (state.Value == null)
            {
                return this.NotFound();
            }
            var result = state.Value.Order;

            Console.WriteLine($"Retrieved order {result.Id} ");

            return result;
        }
    }
}
