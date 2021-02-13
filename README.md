# ErabliereApi
API Pour receuillir et centraliser les informations des appeils dans des érablières.

Démo : https://erabliereapi.freddycoder.com

Documentation swagger : https://erabliereapi.freddycoder.com/api/index.html

## But
Le but de ce projet est de centraliser l'informations des érablières dans le but d'analyser, lever des alertes et automatiser certaine mecanisme.

L'information pourrait aussi bien venir d'appeil ayant la capacité de faire des requêtes http ou d'interaction humaine. Intégrer des visualisations de caméra est aussi planifié.

## Structure
- ErabliereAPI : Projet de l'api dotnet core
- ErabliereIU : Application angular pour l'affichage des données
- ErabliereModel : Classes métiers représentant les modèles de données
- Infrastructure : Fichier yaml pour le déploiement kubernetes
- GenerateurDonneePython : Script python executer sur un raspberry pi pour simulé l'environnement

## Modèles de données
Dans un premier temps, les informations enregistrés seront les suivantes :

- Temperature, Vaccium, Niveau du bassin (Données envoyé automatiquement par l'automate)
- Les dompeux (Entré manuellement depuis un ordinateur client)
- Informations sur les barils (Entré manuellement depuis un ordinateur client)
- Érablière. Noeud racine de la structure de donnée

## Utilisation

Ce projet est utilisable de différente manière :
1. Rouler directement dans un environnement de développement.
2. Déployer sur un PC avec le dotnet core runtime d'installé
2. Utilisation avec Docker
3. Utilisation avec Kubernetes

## Persistance des données

Deux mode sont possible. 

1. Mode en mémoire (aucune persistance, avec swagger il est possible de télécharger les données sous format json et de les stocker manuellement)
2. Sql avec EntityFramework ( Voir le readme dans ./ErabliereApi/Depot/Sql/Readme.md )

## TODO

- Générer des données de test plus réaliste pour le développement
- Créer le section des alertes
- Créer la section des caméra

> Voir la section projet dans github.

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

### Déployer l'interface sur une installation apache2 d'un raspberry pi

> Image utilisé Ubuntu server 20.04 32 bits

```bash
cd ErabliereIU
npm install
ng build --prod
sudo rm /var/www/html/*
sudo cp dist/ErabliereIU/* /var/www/html/
```
