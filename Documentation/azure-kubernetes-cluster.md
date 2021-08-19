<h1>
    <p style="text-align: center;background-color:Gray;color: white">Azure Kubernetes Cluster Setup and Docker Container Deployment</p>
</h1>

- [1. Introduction](#1-Introduction)
- [2. Pre-Requisites](#2-Pre-Requisites)
- [3. Creating AKS and permissions](#3-Creating-AKS-and-permissions)
  - [1. Create a AAD service principal (Why service principal?)](#1-Create-a-AAD-service-principal-Why-service-principal)
  - [2. Configure ACR Authentication](#2-Configure-ACR-Authentication)
  - [3. Create Azure kubernetes Cluster:](#3-Create-Azure-kubernetes-Cluster)
  - [4. Install the Kubernetes CLI](#4-Install-the-Kubernetes-CLI)
  - [5. Connect to cluster using kubectl](#5-Connect-to-cluster-using-kubectl)
  - [6. Appendix](#6-Appendix)

# 1. Introduction

This document makes an effort to explain what needs to be done in order to setup Azure kubernetes cluster and to deploy docker container from Azure Container Registry.

# 2. Pre-Requisites

- Azure account with subscription.
- Azure CLI installed on the local machine. (Required during development)
- 'kubectl' to be installed on the local machine (if not already installed, Required during development)
- Basic knowledge of docker containers and azure kubernetes (Required during development)

# 3. Creating AKS and permissions

## 1. Create a AAD service principal (Why service principal?)

An Azure Active Directory service principal is used by the AKS cluster to interact with any of the Azure resources, ACR (Azure Container registry) for example, from where AKS can pull the docker images. This service principal can be automatically created by the Azure CLI or portal, or you can pre-create one and assign additional permissions. Create a service principal using the az ad sp create-for-rbac command. The --skip-assignment parameter limits any additional permissions from being assigned. By default, this service principal is valid for one year.

**NOTE:** _Make a note of the appId and password. These values are used in the following steps._

```
az ad sp create-for-rbac --skip-assignment --subscription <Azure-Subscription-Id> --name rbac-sp-kubernetes

Example:

az ad sp create-for-rbac --skip-assignment --subscription d3b30a20-e0a9-492a-ac0a-ad7392b0de92 --name rbac-sp-kubernetes

Output:
 {
  "appId": "162c09ff-3acd-413e-8b65-0bcae22b0565",
  "displayName": "azure-cli-2019-05-16-11-46-45",
  "name": "http://azure-cli-2019-05-16-11-46-45",
  "password": "644c93d9-b61b-4e1c-af06-bc6c76ae22f2",
  "tenant": "50de37ea-2f02-41e7-a326-c5610ac7f40c"
}
```

## 2. Configure ACR Authentication

To access images stored in ACR, you must grant the AKS service principal the correct rights to pull images from ACR.

First, get the ACR resource ID using az acr show. Update the `acrName` registry name to that of your ACR instance and the resource group where the ACR instance is located.

```
az acr show --resource-group myResourceGroup --name <acrName> --query "id" --output tsv --subscription <Azure-Subscription-Id>

Example:
az acr show --resource-group dev --name AzureDevACR --query "id" --output tsv --subscription d3b30a20-e0a9-492a-ac0a-ad7392b0de92
```

To grant the correct access for the AKS cluster to pull images stored in ACR, assign the AcrPull role using the az role assignment create command. Replace `appId` and `acrId` with the values gathered in the previous two steps.

```
az role assignment create --assignee <appId> --scope <acrId> --role acrpull --subscription <Azure-Subscription-Id>

Example:
az role assignment create --assignee 162c09ff-3acd-413e-8b65-0bcae22b0565 --scope /subscriptions/d3b30a20-e0a9-492a-ac0a-ad7392b0de92/resourceGroups/dev/providers/Microsoft.ContainerRegistry/registries/AzureDevACR --role acrpull --subscription d3b30a20-e0a9-492a-ac0a-ad7392b0de92
```

## 3. Create Azure kubernetes Cluster:

AKS clusters can use Kubernetes role-based access controls (RBAC). These controls let you define access to resources based on roles assigned to users. Permissions are combined if a user is assigned multiple roles, and permissions can be scoped to either a single namespace or across the whole cluster. By default, the Azure CLI automatically enables RBAC when you create an AKS cluster.

Create an AKS cluster using az aks create.

```
az aks create --resource-group <Resource-Group-Name> --name <AKS-Cluster-Name> --node-count <Number> --service-principal <appId> --client-secret <password> --generate-ssh-keys --subscription <Azure-Subscription-Id> --enable-addons monitoring

Example:

az aks create --resource-group dev --name devAKSApiCluster --node-count 1 --service-principal 162c09ff-3acd-413e-8b65-0bcae22b0565 --client-secret 644c93d9-b61b-4e1c-af06-bc6c76ae22f2 --generate-ssh-keys --subscription d3b30a20-e0a9-492a-ac0a-ad7392b0de92 --enable-addons monitoring
```

## 4. Install the Kubernetes CLI

To connect to the Kubernetes cluster from your local computer, you use kubectl, the Kubernetes command-line client.

If you use the Azure Cloud Shell, kubectl is already installed. You can also install it locally using the az aks install-cli command:

```
az aks install-cli
```

## 5. Connect to cluster using kubectl

To configure kubectl to connect to your Kubernetes cluster, use the az aks get-credentials command.

```
az aks get-credentials --resource-group <Resource-Group-Name> --name <AKS-Cluster-Name> --subscription <AZ-SUBSCRIPTION>

Example:
az aks get-credentials --resource-group dev --name devAKSApiCluster --subscription d3b30a20-e0a9-492a-ac0a-ad7392b0de92
```

## 6. Appendix

- Azure and Aks using CLI:

  https://docs.microsoft.com/en-us/azure/aks/azure-ad-integration-cli

  https://docs.microsoft.com/en-us/cli/azure/ad/sp?view=azure-cli-latest#az-ad-sp-create-for-rbac

- Azure pod managed identity

  For using managed identity with the Azure kubernetes pods refer to the url: https://github.com/Azure/aad-pod-identity

- Azure Extension Preview (https://github.com/Azure/azure-cli-extensions/tree/master/src/aks-preview)

  To install Azure extension preview run the command :

```
  az extension add --name aks-preview
```
