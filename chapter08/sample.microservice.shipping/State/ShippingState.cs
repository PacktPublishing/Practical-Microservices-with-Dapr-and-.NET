using System;
using System.Collections.Generic;

namespace sample.microservice.state.shipping
{
    public class ShippingState
    {
        public DateTime CreatedOn {get; set;}
        public DateTime UpdatedOn {get; set;}
        public Guid OrderId { get; set; }
        public Guid ShipmentId { get; set; }
    }
}