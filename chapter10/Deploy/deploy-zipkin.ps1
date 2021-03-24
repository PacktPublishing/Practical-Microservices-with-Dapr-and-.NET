# create zipkin deployment & service
kubectl apply -f .\Deploy\zipkin.yaml

# create Dapr configuration to use zipkin service 
kubectl apply -f .\Deploy\configuration-zipkin.yaml

#you might have zipkin already installed locally, in such case you should use a different local port as 9412
kubectl port-forward svc/zipkin 9412:9411

kubectl logs -l app=zipkin -c zipkin --namespace default -f