using System;
using System.Collections.Generic;

namespace sample.microservice.dto.reservation
{

    public class OrderReservation
    {
        public Guid OrderId { get; set; }

        public List<Item> ReservedItems { get; set; }
        
    }
}