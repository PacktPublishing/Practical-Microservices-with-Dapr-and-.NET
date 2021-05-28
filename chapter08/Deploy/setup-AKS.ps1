az login
az account set --subscription "Sandbox"

# create RG
az group create --name daprk8srgdb  --location northeurope

# create AKS --- no cluster autoscaler https://docs.microsoft.com/en-us/azure/aks/start-stop-cluster  
az aks create --resource-group daprk8srgdb --name daprk8saksdb `
    --node-count 3 --node-vm-size Standard_D2s_v3 `
    --enable-addons monitoring `
    --vm-set-type VirtualMachineScaleSets `
    --generate-ssh-keys

# stop AKS --- https://docs.microsoft.com/en-us/azure/aks/start-stop-cluster
az aks stop --name daprk8saksdb --resource-group daprk8srgdb
az aks show --name daprk8saksdb --resource-group daprk8srgdb

# start AKS --- https://docs.microsoft.com/en-us/azure/aks/start-stop-cluster 
az aks start --name daprk8saksdb --resource-group daprk8srgdb
az aks show --name daprk8saksdb --resource-group daprk8srgdb

# kubectl CLI
az aks install-cli

# access AKS
az aks get-credentials --name daprk8saksdb --resource-group daprk8srgdb
# query AKS
kubectl get nodes
