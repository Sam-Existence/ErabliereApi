# ErabliereApi
Solution de monitoring pour érablière. Contient un REST API ainsi qu'un application web pour la gestion des données et d'autre script permettant de connecter divers appareils.

L'application est accessible à l'url: https://erabliereapi.freddycoder.com/ une authentification est nécessare pour la majorité des fonctionnalités. POur obtenir un compte, veuillez accéder au instruction dans la section À propos.

Un démo est accessible à l'url: https://erabliereapi-demo.azurewebsites.net/ qui ne nécessite pas d'authentification.

## But
Le but de ce projet est d'analyser, lever des alertes et automatiser certain mecanisme. Basé sur les données receuillis et de façon centralisé.

L'information pourrait aussi bien venir d'appeil ayant la capacité de faire des requêtes http ou d'interaction humaine.

## Structure

### Diagramme de haut niveau de la solution déployé dans AKS

![Architecture Diagram](https://github.com/freddycoder/ErabliereApi/blob/master/Diagrams/ErabliereApi.drawio.png?raw=true)

### Dossier du repository
- ErabliereAPI : Projet du web API
- ErabliereIU : Application angular pour l'affichage des données
- ErabliereModel : Projet du modèles de données
- ErabliereApi.Proxy : Proxy pour le web API disponible soous forme de nuget
- Infrastructure : Fichier relié à la configuration de l'infrastructure	kubernetes ou autres
- PythonScripts : Script python pour alimenter l'API
- IdentityServer/ErabliereApi.ServeurIdentite : Server de jeton OIDC

## Modèles de données
Les informations enregistré peuvent être :

- Érablière. Noeud racine de la structure de donnée
- Capteurs. Représente un capteur
- DonneeCapteur. Une donnée d'une capteur
- Temperature, Vaccium, Niveau du bassin (Données extraire par un script depuis un image d'un HMI)
- Les dompeux (Capturer à l'aide d'un capteur de mouvenement)
- Informations sur les barils (Entré manuellement depuis un ordinateur client)

## Utilisation

Ce projet est utilisable de différente manière :
1. Rouler directement dans un environnement de développement.
2. Déployer sur un PC avec le .net 7 runtime d'installé
2. Utilisation avec Docker
3. Utilisation avec Kubernetes

Une image docker est disponible sur dockerhub:

```
docker pull erabliereapi/erabliereapi
docker run -d -p 9001:80 erabliereapi/erabliereapi
```

Une librairie proxy est disponible sur nuget.org:

.NET 7
```
<PackageReference Include="ErabliereAPI.Proxy" Version="2.0.1" />
```

.NET 6
```
<PackageReference Include="ErabliereAPI.Proxy" Version="1.1.1" />
```

## Persistance des données

Deux mode sont possible. 

1. Mode en mémoire (aucune persistance, avec swagger il est possible de télécharger les données sous format json et de les stocker manuellement)
2. Sql avec EntityFramework ( Voir le readme dans https://github.com/freddycoder/ErabliereApi/tree/master/ErabliereApi/Depot/Sql )

## Documentation additionnelle

### Lancer les scripts de génération de donnée python pour le développement avec cron

La cronjob suivante va lancer le script de génération de donnée pour 2 érablière en utilisant l'api de l'adresse spécifié

```
crontab -e
*/1 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/donnees.py 2 http://192.168.0.103:5000
```

### Extraire les logs sauf pour certain paramètre

```bash
kubectl logs --since=24h pods/my-nginx-deployment-5977f4fdff-p7t5r | grep erabliere | grep -i -v 'param1|param2'
```

### Compiler l'image docker

À la racine du repo, executer ```docker build -t erabliereapi:local .```

### Déployer la solution avec docker desktop

Prerequis: Powershell core : https://learn.microsoft.com/fr-fr/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3#installing-the-msi-package

Avec powershell core en tant qu'administrateur executer le script ```.\deploiement-local.ps1``` puis ensuite ```docker compose up -d```. Pour mettre à jour un déploiement docker compose, executez ```docker compose up -d --force-recreate```. Si vous voulez télécharger les images plus récente, lancer ```docker compose pull``` avant d'executer la commande --force-recreate.

### Intégration Stripe

Dans la version 3, qui est en développement, l'api offre une intégration avec Stripe. Pour utiliser Stripe, il faut initialiser quelque variable d'environnement :

```
  "Stripe.ApiKey": "sk_test_...",
  "Stripe.SuccessUrl": "https://...",
  "Stripe.CancelUrl": "https://...",
  "Stripe.BasePlanPriceId": "price_...",
  "Stripe.WebhookSecret": "secure-...",
  "Stripe.WebhookSiginSecret": "whsec_...",
```

Et démarrer le stripe CLI dans un terminal :

> Sur windows, il faut télécharger l'executable et s'assurer qu'il soit disponible dans le PATH.
> Télécharger le CLI de stripe : https://github.com/stripe/stripe-cli/releases/

La première fois, il faut se connecter avec ```stripe login```.
```
stripe login
stripe listen --forward-to localhost:5000/Checkout/Webhook
dotnet user-secrets set Stripe.WebhookSiginSecret <webhook-signing-secret-from-previous-command>
```

> Plus d'information: https://stripe.com/docs/webhooks/test

Notez que les configuration courriels doivent être configuré pour que les utilisateurs puissent recevoir leur clé d'api. Les configurations courriel sont documenté ici: https://github.com/freddycoder/ErabliereApi/tree/master/Infrastructure#fonctionnalit%C3%A9-dalerte

### Déployer l'interface sur une installation apache2 d'un raspberry pi

> Image utilisé Ubuntu server 20.04 32 bits

```bash
cd ErabliereIU
npm install
ng build --prod
sudo rm -r /var/www/html/*
sudo cp -r dist/ErabliereIU/* /var/www/html/
```

### Documentation sur les configuration réseau ubuntu server

https://netplan.io/examples/

### Configuration office365

https://www.powershellgallery.com/packages/ExchangeOnlineManagement/2.0.4

https://docs.microsoft.com/en-us/powershell/exchange/connect-to-exchange-online-powershell?view=exchange-ps

### Utiliser ubuntu server derrière une connexion internet limité

> Utiliser seulement derrière des connexion internet limité

Ubuntu effectue des mises à jour de sécurité en arrière plan et peut avoir un impacte sur le nombre de donnée échangé par le système d'expoitation et l'ordinateur.

```
sudo systemctl disable apt-daily.timer
sudo systemctl disable apt-daily-upgrade.timer
```
