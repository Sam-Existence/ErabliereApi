# ErabliereApi
Solution de monitoring pour érablière. Contient un REST API ainsi qu'un application web pour la gestion des données et d'autre script permettant de connecter divers appareils.

## But
Le but de ce projet est d'analyser, lever des alertes et automatiser certaine mecanisme. Basé sur les données receuillis et de façon centralisé.

L'information pourrait aussi bien venir d'appeil ayant la capacité de faire des requêtes http ou d'interaction humaine.

## Structure

### Diagramme de haut niveau de la solution

![Architecture Diagram](https://github.com/freddycoder/ErabliereApi/blob/master/ErabliereApi.drawio.png?raw=true)

### Dossier du repository
- ErabliereAPI : Projet du web API
- ErabliereIU : Application angular pour l'affichage des données
- ErabliereModel : Projet du modèles de données
- Infrastructure : Fichier yaml pour le déploiement kubernetes
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
2. Déployer sur un PC avec le dotnet core runtime d'installé
2. Utilisation avec Docker
3. Utilisation avec Kubernetes

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

Prerequis: Powershell core : https://docs.microsoft.com/fr-fr/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2#installing-the-msi-package

Avec powershell core en tant qu'administrateur executer le script ```.\deploiement-local.ps1``` puis ensuite ```docker compose up -d```. Pour mettre à jour un déploiement docker compose, executez ```docker compose up -d --force-recreate```.

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

### Utiliser ubuntu server derière une connexion internet limité

> Utiliser seulement derière des connexion internet limité

Ubuntu effectue des mises à jour de sécurité en arrière plan et peut avoir un impacte sur le nombre de donnée échangé par le système d'expoitation et l'ordinateur.

```
sudo systemctl disable apt-daily.timer
sudo systemctl disable apt-daily-upgrade.timer
```
