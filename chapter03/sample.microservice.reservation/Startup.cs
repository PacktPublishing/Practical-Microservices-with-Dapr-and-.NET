using System;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace sample.microservice.reservation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDaprClient();
            
            services.AddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, JsonSerializerOptions serializerOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("reserve", Reserve);
            });

            async Task Reserve(HttpContext context)
            {
                Console.WriteLine("Enter Reservation");
                
                var client = context.RequestServices.GetRequiredService<DaprClient>();

                var item = await JsonSerializer.DeserializeAsync<Item>(context.Request.Body, serializerOptions);
                
                // your business logic should be here

                /* a specific type is used in sample.microservice.reservation and not
                reused the class in sample.microservice.order with the same signature: 
                this is just to not introduce DTO and to suggest that it might be a good idea
                having each service separating the type for persisting store */
                Item storedItem;
                // from store? state?
                storedItem = new Item();
                storedItem.SKU = item.SKU;
                storedItem.Quantity -= item.Quantity;

                Console.WriteLine($"Reservation of {storedItem.SKU} is now {storedItem.Quantity}");

                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, storedItem, serializerOptions);
            }
        }
    }
}
