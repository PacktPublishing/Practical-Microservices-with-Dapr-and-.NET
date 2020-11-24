# Create a namespace for your ingress resources
kubectl create namespace ingress-basic

# Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Use Helm to deploy an NGINX ingress controller
helm install nginx-ingress ingress-nginx/ingress-nginx `
    --namespace ingress-basic `
    --set controller.replicaCount=2 `
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-dns-label-name"="dapringresssdb"

# verify
kubectl --namespace ingress-basic get services -o wide -w nginx-ingress-ingress-nginx-controller

# create ingress 
kubectl apply -f .\Deploy\ingress-order.yaml
kubectl apply -f .\Deploy\ingress-reservation.yaml