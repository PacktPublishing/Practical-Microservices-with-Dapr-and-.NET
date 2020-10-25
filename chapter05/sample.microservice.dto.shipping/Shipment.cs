using System;
using System.Collections.Generic;

namespace sample.microservice.dto.shipping
{
    public class Shipment
    {
        public DateTime Date { get; set; }

        public Guid OrderId { get; set; }

        public string CustomerCode { get; set; }

        public int NumberOfItems { get; set; }
    }
}