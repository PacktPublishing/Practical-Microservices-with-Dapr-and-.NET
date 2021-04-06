dapr run --app-id "shipping-service" --app-port "5005" --dapr-grpc-port "50050" --dapr-http-port "5050" --components-path "./components" -- dotnet run --project ./sample.microservice.shipping/sample.microservice.shipping.csproj --urls="http://+:5005"

dapr publish --pubsub commonpubsub -t OnOrder_Prepared -d '"{\"OrderId\": \"08ec11cc-7591-4703-bb4d-7e86787b64fe\"}"'