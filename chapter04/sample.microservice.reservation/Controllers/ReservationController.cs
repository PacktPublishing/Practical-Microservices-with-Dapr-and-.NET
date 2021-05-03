using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using sample.microservice.dto.reservation;
using sample.microservice.state.reservation;

namespace sample.microservice.reservation.Controllers
{
    [ApiController]
    public class ReservationController : ControllerBase
    {
        public const string StoreName = "reservationstore";

        /// <summary>
        /// Method for reserving an item quantity.
        /// </summary>
        /// <param name="reservation">Reservation info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("reserve")]
        public async Task<ActionResult<Item>> Reserve(Item reservation, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter item reservation");
            
            var state = await daprClient.GetStateEntryAsync<ItemState>(StoreName, reservation.SKU);
            state.Value ??= new ItemState() { SKU = reservation.SKU, Changes = new List<ItemReservation>() };

            // update balance
            state.Value.BalanceQuantity -= reservation.Quantity;

            // record change
            ItemReservation change = new ItemReservation() { SKU = reservation.SKU, Quantity = reservation.Quantity, ReservedOn = DateTime.UtcNow };
            state.Value.Changes.Add(change);
            if (state.Value.Changes.Count > 10) state.Value.Changes.RemoveAt(0);

            await state.SaveAsync();

            // return current balance
            var result = new Item() {SKU = state.Value.SKU, Quantity= state.Value.BalanceQuantity};

            Console.WriteLine($"Reservation of {result.SKU} is now {result.Quantity}");

            return result;
        }

        /// <summary>
        /// Method for reserving an item quantity.
        /// </summary>
        /// <param name="reservation">Reservation info.</param>
        /// <param name="daprClient">State client to interact with Dapr runtime.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("balance/{state}")]
        public ActionResult<Item> Get([FromState(StoreName)]StateEntry<ItemState> state, [FromServices] DaprClient daprClient)
        {
            Console.WriteLine("Enter item retrieval");
            
            if (state.Value == null)
            {
                return this.NotFound();
            }
            var result = new Item() {SKU = state.Value.SKU, Quantity= state.Value.BalanceQuantity};

            Console.WriteLine($"Retrieved {result.SKU} is {result.Quantity}");

            return result;
        }
    }
}
