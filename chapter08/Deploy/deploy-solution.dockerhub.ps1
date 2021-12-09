# deploy Dapr components
kubectl apply -f .\Deploy\component-pubsub.yaml
kubectl apply -f .\Deploy\component-reservationinput.yaml
kubectl apply -f .\Deploy\component-state-order.yaml
kubectl apply -f .\Deploy\component-state-reservation.yaml
kubectl apply -f .\Deploy\component-state-reservationitem.yaml
kubectl apply -f .\Deploy\component-state-shipping.yaml
kubectl apply -f .\Deploy\component-state-customization.yaml

# deploy Dapr application, using containers images from DockerHub
kubectl apply -f .\Deploy\sample.microservice.order.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.reservation.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.reservationactor.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.customization.dockerhub.yaml
kubectl apply -f .\Deploy\sample.microservice.shipping.dockerhub.yaml

# deploy service of applications ONLY if using classic NGINX --- NOT NEEDED if using NGINX + Dapr
kubectl apply -f .\Deploy\service.sample.microservice.order.yaml
kubectl apply -f .\Deploy\service.sample.microservice.reservation.yaml

# a simple way to test if an application is working
kubectl logs -l app=reservationactor-service -c reservationactor-service --namespace default -f