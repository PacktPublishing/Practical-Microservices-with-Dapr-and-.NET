using System;
using System.Threading.Tasks;
using Dapr;
using Dapr.Client;
using Dapr.Client.Http;
using Microsoft.AspNetCore.Mvc;

namespace sample.microservice.order.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
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

            HTTPExtension httpExtension = new HTTPExtension()
            {
                Verb = HTTPVerb.Post
            };
            foreach (var item in order.Items)
            {
                /* a dynamic type is passed to sample.microservice.reservation and not
                the Order in scope of sample.microservice.order, you could use DTO instead */
                var data = new { sku = item.ProductCode, quantity = item.Quantity };
                var result = await daprClient.InvokeMethodAsync<object, dynamic>("reservation-service", "reserve", data, httpExtension);
                Console.WriteLine($"sku: {result.GetProperty("sku")} === new quantity: {result.GetProperty("quantity")}");
            }
            
            Console.WriteLine($"Submitted order {order.Id}");

            return order.Id;
        }
    }
}
