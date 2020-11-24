using Dapr.Actors;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace sample.microservice.state.reservationactor
{
    public class ItemState
    {
        public string SKU {get; set;}
        public int BalanceQuantity { get; set; }
        public List<ItemReservation>? Changes { get; set; }
    }

    public class ItemReservation
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public DateTime ReservedOn { get; set; }
    }
}