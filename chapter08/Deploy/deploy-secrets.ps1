kubectl delete secret cosmosdb-secret
kubectl create secret generic cosmosdb-secret --from-literal=masterKey='#secret#' --from-literal=url='#secret#'
kubectl delete secret servicebus-secret
kubectl create secret generic servicebus-secret --from-literal=connectionString='#secret#'