# order-service HTTP + reservation-service HTTP
dapr run --app-id "order-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --project ./sample.microservice.order/sample.microservice.order.csproj --urls="http://+:5001"
dapr run --app-id "reservation-service" --app-port "5002" --dapr-grpc-port "50020" --dapr-http-port "5020" -- dotnet run --project ./sample.microservice.reservation/sample.microservice.reservation.csproj --urls="http://+:5002"

# order-service HTTP + reservation-service gRPC
dapr run --app-id "order-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --project ./sample.microservice.order-grpc-client/sample.microservice.order-grpc-client.csproj --urls="http://+:5001"
dapr run --app-id "reservation-service" --app-port "3000" --app-protocol grpc --dapr-http-port "5020" --dapr-grpc-port "50020" -- dotnet run --project ./sample.microservice.reservation-grpc/sample.microservice.reservation-grpc.csproj --urls="http://+:3000"