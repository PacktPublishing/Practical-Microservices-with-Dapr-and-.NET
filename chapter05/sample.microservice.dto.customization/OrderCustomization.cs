using System;
using System.Collections.Generic;

namespace sample.microservice.dto.customization
{
    public class OrderCustomization
    {
        public Guid OrderId { get; set; }

        public List<ItemCustomization> CustomizedItems { get; set; }
    }
    
    public class ItemCustomization
    {
        public string SKU {get; set;}
        public int Quantity { get; set; }
        public Guid CustomizationId { get; set; }

        public bool Succeeded { get; set; }
    }
}
