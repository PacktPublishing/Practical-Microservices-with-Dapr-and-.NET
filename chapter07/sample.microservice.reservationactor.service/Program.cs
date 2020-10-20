using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Dapr.Actors.AspNetCore;
using Microsoft.AspNetCore;

namespace sample.microservice.reservationactor.service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseActors(actorRuntime =>
                {
                    // Register ReservationItemActor actor type
                    actorRuntime.RegisterActor<ReservationItemActor>();
                });
    }
}
