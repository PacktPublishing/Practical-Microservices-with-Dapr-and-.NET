using System;
using System.Collections.Generic;

namespace sample.microservice.state.reservation
{
    public class ReservationState
    {
        public Guid OrderId { get; set; }
        public List<ItemReservation>? ReservedItems { get; set; }
    }
    
}