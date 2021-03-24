# this deploys open telemetry collector exporter to applciation insights
kubectl apply -f .\Deploy\open-telemetry-collector.yaml
# this configures open telemetry for tracing
kubectl apply -f .\Deploy\configuration-open-telemetry.yaml
