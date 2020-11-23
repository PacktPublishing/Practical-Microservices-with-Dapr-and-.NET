kubectl create namespace dapr-monitoring

# install prometheuse via Helm 3
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
helm install dapr-prom prometheus-community/prometheus -n dapr-monitoring
# verify installation
kubectl get pods -n dapr-monitoring

# install grafana via Helm 3
helm repo add grafana https://grafana.github.io/helm-charts
helm repo update
helm install grafana grafana/grafana -n dapr-monitoring
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