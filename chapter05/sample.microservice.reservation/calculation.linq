<Query Kind="Program" />

// You can also write your own methods and classes. Just change the language dropdown to 'C# Program'.
// LINQPad will automatically generate a 'Main' method.

void Main()
{
	var reservedItems = new List<ItemCustomization>();
	reservedItems.Add(new ItemCustomization(){SKU = "A", Quantity=1});
    reservedItems.Add(new ItemCustomization(){SKU = "A", Quantity=2});
    reservedItems.Add(new ItemCustomization(){SKU = "B", Quantity=2});
	reservedItems.Dump();
	
	var groupedCustomized = 
		from item in reservedItems
		group item by item.SKU into items
		select new 
		{
			SKU = items.Key,
			Quantity = items.Sum(x => x.Quantity)
		};
	groupedCustomized.Dump();
	
	var allItems = new List<ItemCustomization>();
	allItems.Add(new ItemCustomization(){SKU = "A", Quantity=2});
    allItems.Add(new ItemCustomization(){SKU = "A", Quantity=2});
    allItems.Add(new ItemCustomization(){SKU = "B", Quantity=1});
    allItems.Add(new ItemCustomization(){SKU = "B", Quantity=3});
    allItems.Add(new ItemCustomization(){SKU = "C", Quantity=1});
	allItems.Add(new ItemCustomization(){SKU = "C", Quantity=2});
	allItems.Dump();
	
	var groupedReserved = 
		from item in allItems
		group item by item.SKU into items
		select new 
		{
			SKU = items.Key,
			Quantity = items.Sum(x => x.Quantity)
		};
	groupedReserved.Dump();
	
	foreach (var itemReserved in groupedReserved)
    {
        var x = groupedCustomized.Where(e => e.SKU == itemReserved.SKU).FirstOrDefault();
		//var z = (x == null ? 0 : x.Quantity);
		(itemReserved.Quantity - (x == null ? 0 : x.Quantity)).Dump();
		//var y = groupedCustomized.FirstOrDefault(e => e.SKU == itemReserved.SKU);
		//y.Dump();
		//var x = customizedItem.DefaultIfEmpty(new ItemCustomization(){SKU = itemReserved.SKU, Quantity = 0})
    }

}

public class ItemCustomization
{
    public string SKU {get; set;}
    public int Quantity { get; set; }
    public Guid CustomizationId { get; set; }
}
