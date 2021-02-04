# ErabliereApi
API Pour receuillir et centraliser les informations des appeils dans des érablières.

## But
Le but de ce projet est de centraliser l'informations des érablières dans le but d'analyser les données dans un deuxième temps.

## Structure
- ErabliereAPI : Projet de l'api dotnet core
- ErabliereIU : Application angular pour l'affichage des données
- ErabliereModel : Classes métiers représentant les modèles de données
- Infrastructure : Fichier yaml pour le déploiement kubernetes
- GenerateurDeDonnées : Application console pour générer des données de test

## Modèles de données
Dans un premier temps, les informations enregistrés seront les suivantes :

- Temperature, Vaccium, Niveau du bassin (Données envoyé automatiquement par l'automate)
- Les dompeux (Entré manuellement depuis un ordinateur client)
- Informations sur les barils (Entré manuellement depuis un ordinateur client)

## Utilisation

Ce projet est utilisable de différente manière :
1. Rouler directement dans un environnement de développement.
2. Déployer sur un PC avec le dotnet core runtime d'installé
2. Utilisation avec Docker
3. Utilisation avec Kubernetes

> Présentement, l'utilisation d'un dépôt de données en mémoire et de swagger est la seule option possible.

## TODO

- Ajout d'un dépôt de données persistant. (Fichier, MongoBd, MsSql, etc.).
- Développer les interfaces graphique pour afficher des graphiques et effectuer l'entré de données manuel.
- Générer des données de test plus réaliste pour le développement
- Déployer l'api dans le cloud (github action)
- Générer un artéfact sous forme de fichier zip contenant l'application IU
