## Note sur les fichier infrastructure

### Générer un secret pour la base de donnée 

```
kubectl create secret generic mssql --from-literal=SA_PASSWORD="V3ryStr0ngPa55!" --namespace=erabliere-api
```

### Déployer dans un nouveau cluster

Pour un mode sans persistance des données : 
1. Api

Pour un mode avec de la persistance des données :
1. Storage.
2. Database.
3. Api.

### Les configurations de la BD sont basé sur ce cours

https://app.pluralsight.com/library/courses/microsoft-azure-deploying-sql-server-containers