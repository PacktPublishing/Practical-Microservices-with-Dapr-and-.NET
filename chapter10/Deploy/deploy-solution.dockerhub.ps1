# these deployments use the images from DockerHub
kubectl apply -f .\Deploy\sample.microservice.order.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.reservation.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.reservationactor.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.customization.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.shipping.dockerhub.yaml

kubectl logs -l app=reservationactor-service -c reservationactor-service --namespace default -f

# verify horizontal autoscaler
kubectl get hpa -w