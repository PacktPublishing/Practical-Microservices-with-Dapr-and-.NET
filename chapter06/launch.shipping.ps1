# use this command to start the shipping-service app
dapr run --app-id "shipping-service" --app-port "5005" --dapr-grpc-port "50050" --dapr-http-port "5050" --components-path "./components" -- dotnet run --project ./sample.microservice.shipping/sample.microservice.shipping.csproj --urls="http://+:5005"

# replace OrderId with an existing order
dapr publish --publish-app-id "shipping-service" --pubsub commonpubsub -t OnOrder_Prepared -d '"{\"OrderId\": \"08ec11cc-7591-4702-bb4d-7e86787b64fe\"}"'