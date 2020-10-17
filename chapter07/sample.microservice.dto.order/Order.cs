using System;
using System.Collections.Generic;

namespace sample.microservice.dto.order
{
    public class Order
    {
        public DateTime Date { get; set; }

        public Guid Id { get; set; }

        public string CustomerCode { get; set; }

        public List<OrderItem> Items { get; set; }

        public List<SpecialRequest>? SpecialRequests { get; set; }
    }

    public class OrderItem
    {
        public string ProductCode {get; set;}
        public int Quantity { get; set; }
    }

    public class SpecialRequest
    {
        public OrderItem Scope { get; set; }

        public Guid CustomizationId { get; set; }
    }
}