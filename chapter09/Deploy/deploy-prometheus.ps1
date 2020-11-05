kubectl create namespace dapr-monitoring

# install prometheuse via Helm 3
helm repo add stable https://kubernetes-charts.storage.googleapis.com
helm repo update
helm install dapr-prom stable/prometheus -n dapr-monitoring
# verify installation
kubectl get pods -n dapr-monitoring

# install grafana via Helm 3
helm install grafana stable/grafana -n dapr-monitoring --set persistence.enabled=true

# get 
kubectl get pods -n dapr-monitoring

# get Grafana password as base64
kubectl get secret --namespace dapr-monitoring grafana -o jsonpath="{.data.admin-password}"
# decode Grafana password
$base64secret = kubectl get secret --namespace dapr-monitoring grafana -o jsonpath="{.data.admin-password}"
$password = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($base64secret))
$password
# a sample password
w8O9dMin8nGIn8p9h9tgtTgnd64yQsJ7bnjwQm5A

# get prometheuse service --> Grafana data source http://dapr-prom-prometheus-server.dapr-monitoring
kubectl get svc -n dapr-monitoring

# connect to Grafana 
kubectl port-forward svc/grafana 8090:80 -n dapr-monitoring