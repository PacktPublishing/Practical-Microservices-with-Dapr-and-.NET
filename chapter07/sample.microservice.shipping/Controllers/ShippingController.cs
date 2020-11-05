using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.shipping;
using sample.microservice.common;
using sample.microservice.state.shipping;

namespace sample.microservice.shipping.Controllers
{
    [ApiController]
    public class ShippingController : ControllerBase
    {
        public const string StoreName = "shippingstore";
        public const string PubSub = "commonpubsub";

        /// <summary>
        /// Method for shipping order.
        /// </summary>
        /// <param name="orderShipment">Shipment info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, Topics.OrderPreparedTopicName)]
        [HttpPost("ship")]
        public async Task<ActionResult<Guid>> ship(Shipment orderShipment, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter shipment start");
            
            var state = await daprClient.GetStateEntryAsync<ShippingState>(StoreName, orderShipment.OrderId.ToString());
            state.Value ??= new ShippingState() {OrderId = orderShipment.OrderId, ShipmentId = Guid.NewGuid() };

            await state.SaveAsync();

            // return current balance
            var result = state.Value.ShipmentId;

            Console.WriteLine($"Shipment of orderId {orderShipment.OrderId} completed with id {result}");

            return result;
        }
    }
}
