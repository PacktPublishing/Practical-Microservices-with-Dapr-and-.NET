using System;
using System.Threading.Tasks;
using Dapr.Client;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.order.Models;

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

            foreach (var item in order.Items)
            {
                /* a dynamic type is passed to sample.microservice.reservation and not
                the Order in scope of sample.microservice.order, you could use DTO instead */
                var data = new { sku = item.ProductCode, quantity = item.Quantity };
                var result = await daprClient.InvokeMethodAsync<object, dynamic>(HttpMethod.Post, "reservation-service", "reserve", data);
                Console.WriteLine($"sku: {result.GetProperty("sku")} === new quantity: {result.GetProperty("quantity")}");
            }
            
            Console.WriteLine($"Submitted order {order.Id}");

            return order.Id;
        }
    }
}
