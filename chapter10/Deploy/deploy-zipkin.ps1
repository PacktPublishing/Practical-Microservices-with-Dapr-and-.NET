kubectl apply -f .\Deploy\zipkin.yaml

kubectl apply -f .\Deploy\configuration-zipkin.yaml
kubectl apply -f .\Deploy\component-zipkin.yaml

kubectl port-forward svc/zipkin 9412:9411
kubectl logs -l app=zipkin -c zipkin --namespace default -f