using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.customization;
using sample.microservice.dto.order;

namespace sample.microservice.customization.Controllers
{
    [ApiController]
    public class CustomizationController : ControllerBase
    {
        public const string PubSub = "commonpubsub";
        
        /// <summary>
        /// Method for reserving all items quantity in an order.
        /// </summary>
        /// <param name="order">Order info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [Topic(PubSub, common.Topics.ReservationCompletedTopicName)]
        [HttpPost(common.Topics.ReservationCompletedTopicName)]
        public async Task<ActionResult<OrderCustomization>> CustomizeOrder(Order order, [FromServices] DaprClient daprClient)
        {            
            // no state management in this microservice sample
            
            var result = new OrderCustomization(){OrderId = order.Id, CustomizedItems = new List<ItemCustomization>()};

            // no customization requested
            if (order.SpecialRequests == null || order.SpecialRequests.Count == 0)
            {
                await daprClient.PublishEventAsync<OrderCustomization>(PubSub,common.Topics.CustomizationCompletedTopicName, result);

                Console.WriteLine($"Customization in {order.Id} not requested");
                return this.Ok();
            }

            // in case customization is requested
            foreach (var item in order.SpecialRequests)
            {
                var SKU = item.Scope.ProductCode;
                var quantity = item.Scope.Quantity;

                // simulate chaos is crazycookie is customized
                bool customizationSucceeded = !(SKU.Equals("crazycookie"));
                
                // item passed customized but Succeeded
                var customizedItem = new ItemCustomization(){ SKU = SKU, Quantity= quantity, CustomizationId = item.CustomizationId, Succeeded = customizationSucceeded };
                result.CustomizedItems.Add(customizedItem);

                if (!customizedItem.Succeeded)
                {
                    await daprClient.PublishEventAsync<OrderCustomization>(PubSub,common.Topics.CustomizationFailedTopicName, result);
                    
                    Console.WriteLine($"Customization in {order.Id} of {SKU} for {quantity} failed");
                    return this.NotFound();
                }
                else
                {
                    Console.WriteLine($"Customization in {order.Id} of {SKU} for {quantity}, special request {item.CustomizationId}, done");
                }
            }
        
            await daprClient.PublishEventAsync<OrderCustomization>(PubSub,common.Topics.CustomizationCompletedTopicName, result);

            Console.WriteLine($"Customization in {order.Id} completed");
            return this.Ok();
        }
    }
}
