## Note sur les fichier infrastructure

### Déployer dans un nouveau cluster

Pour un mode sans persistance des données : 
1. Api

Pour un mode avec de la persistance des données :
1. Storage.
2. Database.
3. Api.

### Générer un secret pour la base de donnée 

```
kubectl create secret generic mssql --from-literal=SA_PASSWORD="V3ryStr0ngPa55!" --namespace=erabliere-api
```

Les configurations de la BD sont basé sur ce cours : https://app.pluralsight.com/library/courses/microsoft-azure-deploying-sql-server-containers

### Fonctionnalité d'alerte

Pour que la fonctionnalité d'alerte fonctionne, il faut que des configurations mail soit accessible. Elles doivent être mis en place manuellement.

La variable d'environnement ```EMAIL_CONFIG_PATH``` est utiliser pour obtenir le path du fichier de configuration.

Le fichier de configuration doit ressembler à ceci : 
```
{
  "Sender": "adresse@courriel.com",
  "Email": "adresse@courriel.com",
  "Password": "motDePasse",
  "SmtpServer": "smtp.courriel.com",
  "SmtpPort": 587
}
```

Lors des déploiements k8s, un fichier doit être présent. Pour ce faire, vous pouvez utiliser un secret comme le suivant : 
```
kind: Secret
apiVersion: v1
metadata:
  name: erabliereapi-email-config
  namespace: erabliere-api
type: Opaque
```

Pour transformer le fichier json en base64 utiliser ```base64 <path-fichier-config>```

Remplacer \<base64-string\> par le résultat sur une seule ligne pour obtenir un secret valide pour la fonctionnalité d'alerte

```
kind: Secret
apiVersion: v1
metadata:
  name: erabliereapi-email-config
  namespace: erabliere-api
type: Opaque
data:
  emailConfig.json: <base64-string>
```

### Modifier les configurations du serveur d'identité

Pour modifier les configurations par défaut du serveur d'identité, il faut créer des fichiers de configuration similaire a ceux du projet ErabliereApi.IdentityServer dans votre cluster.

Pour transformer ce fichier json en base64 utiliser ```base64 <path-fichier-config>```

Remplacer \<base64-string\> par le résultat sur une seule ligne pour obtenir un secret valide pour la fonctionnalité d'alerte

> Pour les utilisateur de powerhsell : https://adsecurity.org/?p=478

```
kind: Secret
apiVersion: v1
metadata:
  name: identity-server-clients
  namespace: erabliere-api
type: Opaque
data:
  ErabliereApi.IdentityServer.Config.json: <base64-string>
  ErabliereApi.IdentityServer.Users.json: <base64-string>
```