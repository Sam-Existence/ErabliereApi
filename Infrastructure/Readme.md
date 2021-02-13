## Note sur les fichier infrastructure

### Générer un secret pour la base de donnée 

```
kubectl create secret generic mssql --from-literal=SA_PASSWORD="V3ryStr0ngPa55!" --namespace=erabliere-api
```
