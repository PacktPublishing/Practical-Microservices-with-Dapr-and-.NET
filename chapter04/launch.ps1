dapr run --app-id "order-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" --components-path "./.private" -- dotnet run --project ./sample.microservice.order/sample.microservice.order.csproj --urls="http://+:5001"
dapr run --app-id "reservation-service" --app-port "5002" --dapr-grpc-port "50020" --dapr-http-port "5020" --components-path "./.private" -- dotnet run --project ./sample.microservice.reservation/sample.microservice.reservation.csproj --urls="http://+:5002"

daprd --app-id "reservation-service" --app-port "5002" --components-path "./.private" --dapr-grpc-port "50020" --dapr-http-port "5020" --metrics-port "9092" --placement-address "localhost:6050"
msg="application discovered on port 5002" app_id=reservation-service 