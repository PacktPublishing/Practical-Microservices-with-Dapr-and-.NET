using System;
using System.Collections.Generic;
using sample.microservice.dto.order;

namespace sample.microservice.state.order
{
    public class OrderState
    {
        public DateTime CreatedOn {get; set;}

        public DateTime UpdatedOn {get; set;}
        public Order Order { get; set; }
    }
}