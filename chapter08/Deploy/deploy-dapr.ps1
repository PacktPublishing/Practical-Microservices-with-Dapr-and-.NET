
# initialize Dapr
dapr init -k
# alternative via Helm 3
helm repo add dapr https://dapr.github.io/helm-charts/
helm repo update
kubectl create namespace dapr-system
helm install dapr dapr/dapr --namespace dapr-system

# verify Dapr in AKS
kubectl get pods -n dapr-system -w
kubectl get services -n dapr-system -w
dapr dashboard -k