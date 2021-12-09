# delete and then create the secrets for the Dapr components
kubectl delete secret cosmosdb-secret
kubectl create secret generic cosmosdb-secret --from-literal=masterKey='#secret#' --from-literal=url='#secret#'
kubectl delete secret servicebus-secret
kubectl create secret generic servicebus-secret --from-literal=connectionString='#secret#'
kubectl delete secret reservationinput-secret
kubectl create secret generic reservationinput-secret --from-literal=connectionString='#secret#' --from-literal=storageAccountKey='#secret#'