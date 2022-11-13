## Note sur les fichier infrastructure

### Déployer dans un nouveau cluster

La solution se déploie en un script. Il faut manuellement aller ajouter les applications dans Azure-AD pour complété la configuration corretement.

Pour modifier le déploiement, avant d'executer la dernière commande, vous pouvez modifier les fichiers dans .\AzureAKS-TLS\erabliereapi-v3.

```
git clone https://github.com/freddycoder/AzureAKS-TLS.git
cd AzureAKS-TLS
.\azure-aks-cluster-deployment.ps1 -resourceGroup erabliereapiv3 -location canadaeast -aksClusterName kerabliereapiv3 -namespace erabliereapi-prod -appScriptPath .\erabliereapi-v3\application-deployment.ps1 -useLetsEncryptProd true -customDomain erabliereapi.freddycoder.com
```

> Dans la version 1 et la version 2 du projet Les configurations de la BD sont basé sur ce cours : https://app.pluralsight.com/library/courses/microsoft-azure-deploying-sql-server-containers pour la version 3, la BD est dans Azure SQL avec la version serverless.

### Fonctionnalité d'alerte

> Ceci est géré dans le script de déploiement. Cette section est conserver a des fins de documentation.

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

Pour modifier les configurations par défaut du serveur d'identité, il faut créer des fichiers une configmap similaire à l'exemple suivant :

Utiliser la variable d'environnement SECRETS_FOLDER pour indiquer ou trouver les fichiers.

```
kind: ConfigMap
apiVersion: v1
metadata:
  name: identity-server-clients
  namespace: erabliere-api
data:
  ErabliereApi.IdentityServer.Config.json: |
    ...
  ErabliereApi.IdentityServer.Users.json: |
    ...
```