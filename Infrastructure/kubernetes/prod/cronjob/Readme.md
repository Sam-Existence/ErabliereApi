# kubernetes / prod / cronjob

Pour déployer les jobs, lancer le script powershell vouut dans le dossier Infrastructure\kubernetes\prod\cronjob

Il faudra premièrement se connecter au cluster avec la commande suivante:

```powershell
az login
az aks get-credentials --resource-group erabliereapiv3 --name kerabliereapiv3 --file kubeconfig-ss
```

Pour visualiser et changer de context au besoin, utiliser:

```powershell
kubectl config get-contexts
kubectl config set-context kerabliereapiv3
```

Si le namespace cible n'existe pas, il faut le créer:

```powershell
kubectl create namespace erabliereapi-prod
```

Si votre cluster ne télécharge pas les images automatiquement il la télécharger manuellement:

```powershell
docker pull erabliereapi/extraireinfohmi:latest
```

Pour déployer les informations d'un hmi lancer, deploy-extraireinfohmi-secret.ps1 puis deploy-extraireinfohmi-cronjob.ps1. Les informations demandé seront:

```
Entrer le tenantId:
<guid>
Entrer le clientId:
<guid>                            
Entrer le clientSecret:
<client-secret>
Entrer le apiClientId:
<guid>
Entrer le namespace du secret:
<namespace>
Entrer le apiKey acuweather:
<api-key>

Entrer l'addresse du hmi:
<url>
Entrer l'addresse de l'api:
https://erabliereapi.freddycoder.com
Entrer l'id de l'érablière:
<guid>
Entrer le namespace de la cronjob:
<namespace>
```

```
.\deploy-extraireinfohmi-secret.ps1
.\deploy-extraireinfohmi-cronjob.ps1
```

Pour déployer l'obtention de temperature depuis acuweather lancer deploy-getweather-saint-adalbert-cronjob.ps1. Les informations demandé seront:

```
Entrer la clé de location acuweather:
<location>
Entrer l'addresse de l'api:
https://erabliereapi.freddycoder.com
Entrer l'id du capteur:
<guid>
Entrer le namespace de la cronjob:
erabliereapi-prod
```

```powershell
.\deploy-getweather-saint-adalbert-cronjob.ps1
```