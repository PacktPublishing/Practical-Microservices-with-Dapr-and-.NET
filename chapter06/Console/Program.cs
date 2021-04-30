using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace chapter06
{
class Program
{
    //private const string connectionString = "# REPLACE";
    
    static async Task Main(string[] args)
    {
        Console.WriteLine("Started sender");

        string connectionString = args[0];

        var rnd = new System.Random();
        var cookies = new List<string>{"bussola1", "bussola8", "rockiecookie", "crazycookie", "cookie43"};
        
        // Create a producer client that you can use to send events to an event hub
        await using (var producerClient = new EventHubProducerClient(connectionString))
        {
            do
            {
                // Create a batch of events 
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                for (int i = 0; i < 3; i++)
                {
                    var item = new  
                    {  
                        SKU = cookies[rnd.Next(cookies.Count)],  
                        Quantity = 1 
                    }; 

                    var message = JsonSerializer.Serialize(item);
                    // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                for (int i = 0; i < 7; i++)
                {
                    var item = new  
                    {  
                        SKU = $"cookie{ rnd.Next(1,1000).ToString("D4")}",  
                        Quantity = 1 
                    }; 

                    var message = JsonSerializer.Serialize(item);
                    // Add events to the batch. An event is a represented by a collection of bytes and metadata. 
                    eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message)));
                }

                // Use the producer client to send the batch of events to the event hub
                await producerClient.SendAsync(eventBatch);
                Console.WriteLine($"Sent batch @ {DateTime.Now.ToLongTimeString()}");

                System.Threading.Thread.Sleep(rnd.Next(200,3000));
            } while (true);
        }

    }
}
}
