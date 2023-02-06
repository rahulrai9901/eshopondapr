### Catalog API Documents

> Note : In Secret Store Secrets File URL should start from the root directory.

**Note**: Run command from src folder
>docker image build -t "eshopdapr/basket.api:latest" -f src/BasketAPI/Dockerfile .

**Note**:  Safe way to use local docker image in kubernetes
Make sure <b>catalog.yaml</b>  Deployment has below setting.
> imagePullPolicy: Never

### Important kubernets command

```
kubectl apply -f template_folder
kubectl delete -f template_folder
kubectl get pods --namespace=name
kubectl logs podname --namespace=name
kubectl get service --namespace=name
kubectl describe pod podname --namespace=name
kubectl get ns
kubectl describe pod name
