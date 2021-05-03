using System;
using System.Threading.Tasks;
using Dapr.Client;
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
                var data = new sample.microservice.reservation_grpc.Generated.Item() { SKU = item.ProductCode, Quantity = item.Quantity };
                var result = await daprClient.InvokeMethodGrpcAsync<sample.microservice.reservation_grpc.Generated.Item, sample.microservice.reservation_grpc.Generated.Item>("reservation-service", "reserve", data);
                Console.WriteLine($"sku: {result.SKU} === new quantity: {result.Quantity}");
            }
            
            Console.WriteLine($"Submitted order {order.Id}");

            return order.Id;
        }
    }
}
