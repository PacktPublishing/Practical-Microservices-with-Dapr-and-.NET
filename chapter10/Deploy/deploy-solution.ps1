kubectl apply -f .\Deploy\sample.microservice.order.yaml
kubectl apply -f .\Deploy\sample.microservice.reservation.yaml
kubectl apply -f .\Deploy\sample.microservice.reservationactor.yaml
kubectl apply -f .\Deploy\sample.microservice.customization.yaml
kubectl apply -f .\Deploy\sample.microservice.shipping.yaml

# verify horizontal autoscaler
kubectl get hpa -w