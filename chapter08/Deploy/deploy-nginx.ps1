# Add the ingress-nginx repository
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update

# Use Helm to deploy an NGINX ingress controller
helm install nginx-ingress ingress-nginx/ingress-nginx `
    --namespace default `
    --set controller.replicaCount=2 `
    --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-dns-label-name"="dapringresssdb"

# verify
kubectl --namespace default get services -o wide -w nginx-ingress-ingress-nginx-controller

# create ingress 
kubectl apply -f .\Deploy\ingress-nginx.yaml
