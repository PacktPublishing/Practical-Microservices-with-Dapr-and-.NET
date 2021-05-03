using System;
using System.Threading.Tasks;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace sample.microservice.reservation_grpc
{
    public class ReservationService : AppCallback.AppCallbackBase
    {
        private readonly ILogger<ReservationService> _logger;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public ReservationService(DaprClient daprClient, ILogger<ReservationService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Method {request.Method}");

            var response = new InvokeResponse();
            switch (request.Method)
            {
                case "reserve":
                    var input = request.Data.Unpack<Generated.Item>();
                    var output = await Task.FromResult<Generated.Item>(new Generated.Item() {SKU=input.SKU, Quantity = - input.Quantity});

                    response.Data = Any.Pack(output);
                    break;
                default:
                    Console.WriteLine("Method not supported");
                    break;
            }
            return response;
        }

         /// <inheritdoc/>
        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListInputBindingsResponse());
        }

        /// <inheritdoc/>
        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListTopicSubscriptionsResponse());
        }
    }
}