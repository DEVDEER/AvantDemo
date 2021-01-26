# Kubernetes Setup

## Azure Application Gateway Kubernetes Ingress

**Github Repo:** [Azure/application-gateway-kubernetes-ingress](https://github.com/Azure/application-gateway-kubernetes-ingress)

### Install Ingress Controller as a Helm Chart

The used `agic-helm-config.yaml` contains configurations for the **DEV**-stage.

```
helm repo add application-gateway-kubernetes-ingress https://appgwingress.blob.core.windows.net/ingress-azure-helm-package/
helm repo update
helm install ???? -f agic-helm-config.yaml application-gateway-kubernetes-ingress/ingress-azure --version 1.2.0
```
