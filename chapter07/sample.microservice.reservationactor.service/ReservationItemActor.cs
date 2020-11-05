using Dapr.Actors;
using Dapr.Actors.Runtime;
using System;
using System.Threading.Tasks;
using sample.microservice.reservationactor.interfaces;
using sample.microservice.state.reservationactor;
using System.Collections.Generic;

namespace sample.microservice.reservationactor.service
{
    internal class ReservationItemActor : Actor, IReservationItemActor, IRemindable
    {        
        public const string StateName = "reservationitem";
        
        /// <summary>
        /// Initializes a new instance of ReservationItemActor
        /// </summary>
        /// <param name="actorService">The Dapr.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Dapr.Actors.ActorId for this actor instance.</param>
        public ReservationItemActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            // Provides opportunity to perform some optional setup.
            Console.WriteLine($"Activating actor id: {this.Id}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        protected override Task OnDeactivateAsync()
        {
            // Provides Opporunity to perform optional cleanup.
            Console.WriteLine($"Deactivating actor id: {this.Id}");
            return Task.CompletedTask;
        }

        public async Task<int> AddReservation(int quantity)
        {
            var SKU = this.Id.GetId();

            var state = await this.StateManager.TryGetStateAsync<ItemState>(StateName);
            ItemState value = state.HasValue ? state.Value : new ItemState() {SKU = SKU, Changes = new List<ItemReservation>() };
                    
            // update balance
            var initialBalanceQuantity = value.BalanceQuantity;
            value.BalanceQuantity -= quantity;

            // record change
            ItemReservation change = new ItemReservation() { SKU = SKU, Quantity = quantity, ReservedOn = DateTime.UtcNow };
            value.Changes.Add(change);
            if (value.Changes.Count > 10) value.Changes.RemoveAt(0);
            
            // Data is saved to configured state store implicitly after each method execution by Actor's runtime.
            // Data can also be saved explicitly by calling this.StateManager.SaveStateAsync();
            // State to be saved must be DataContract serializable.
            await this.StateManager.SetStateAsync<ItemState>(
                StateName,  // state name
                value);      // data saved for the named state "my_data"

            Console.WriteLine($"Balance of {SKU} was {initialBalanceQuantity}, now {value.BalanceQuantity}");

            return value.BalanceQuantity;
        }

        public async Task<int> GetBalance()
        {
            // Gets state from the state store.
            var state = await this.StateManager.GetStateAsync<ItemState>(StateName);
            return state.BalanceQuantity;
        }

        /// <summary>
        /// Register MyReminder reminder with the actor
        /// </summary>
        public async Task RegisterReminder()
        {
            await this.RegisterReminderAsync(
                "MyReminder",              // The name of the reminder
                null,                      // User state passed to IRemindable.ReceiveReminderAsync()
                TimeSpan.FromSeconds(5),   // Time to delay before invoking the reminder for the first time
                TimeSpan.FromSeconds(5));  // Time interval between reminder invocations after the first invocation
        }

        /// <summary>
        /// Unregister MyReminder reminder with the actor
        /// </summary>
        public Task UnregisterReminder()
        {
            Console.WriteLine("Unregistering MyReminder...");
            return this.UnregisterReminderAsync("MyReminder");
        }

        // <summary>
        // Implement IRemindeable.ReceiveReminderAsync() which is call back invoked when an actor reminder is triggered.
        // </summary>
        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            Console.WriteLine("ReceiveReminderAsync is called!");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Register MyTimer timer with the actor
        /// </summary>
        public Task RegisterTimer()
        {
            return this.RegisterTimerAsync(
                "MyTimer",                  // The name of the timer
                this.OnTimerCallBack,       // Timer callback
                null,                       // User state passed to OnTimerCallback()
                TimeSpan.FromSeconds(5),    // Time to delay before the async callback is first invoked
                TimeSpan.FromSeconds(5));   // Time interval between invocations of the async callback
        }

        /// <summary>
        /// Unregister MyTimer timer with the actor
        /// </summary>
        public Task UnregisterTimer()
        {
            Console.WriteLine("Unregistering MyTimer...");
            return this.UnregisterTimerAsync("MyTimer");
        }

        /// <summary>
        /// Timer callback once timer is expired
        /// </summary>
        private Task OnTimerCallBack(object data)
        {
            Console.WriteLine("OnTimerCallBack is called!");
            return Task.CompletedTask;
        }
    }
}
