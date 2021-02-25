# Travailler avec entity framework

> Pour ne pas avoir besoin de mettre des efforts sur la partie base de données, il est possible de mettre la variable d'environnement USE_SQL à "false".

source : https://docs.microsoft.com/en-us/ef/core/cli/dotnet

## Outils prérequis

Installer l'outils dotnet ef

```
dotnet tool install --global dotnet-ef
```

Mettre à jour l'outils dotnet ef

```
dotnet tool update --global dotnet-ef
```

## Chaine de connexion

La chaine de connexion doit être spécifié en variable d'environnement. Pour créer les migrations, il faut ajouter la variable d'environnement SQL_CONNEXION_STRING dans le poste de travail. 
L'outils ef ne prend pas en compte les variables dans le fichier launchSettings.json.

## Instruction migration

Les migrations ont été créer ainsi :

> Dans la console de gestion des packages nuget
```
PM> cd .\ErabliereApi
PM> dotnet ef --startup-project . migrations add InitialSchema --output-dir "Depot\\Sql\\Migrations" --namespace "ErabliereApi.Depot.Sql.Migrations"
```

## Update database

Si la variable d'environnement 'SQL_USE_STARTUP_MIGRATION' est à "true", les migrations seront appliquer automatiquement au démarrage de l'api.

Pour effectuer les migrations manuellement, mettre la variable d'environnement 'SQL_USE_STARTUP_MIGRATION' à "false".

## Faire des requêtes sur les logs

Avec la variable d'environnement ```LOG_SQL``` initialisé avec la valeur ```"Console"```, les logs de entity framework seront afficher dans la console. 

Voici quelque grep qui permettent de faire des requête sur les logs.

```
# Afficher les temps en milisecondes
kubectl logs erabliereapi-deployment-54d89d4698-85hhh --namespace=erabliere-api | grep -oE [0-9]+ms | sort | uniq

# Vérifier ce qu'il y a autour de la transaction de 13ms
kubectl logs erabliereapi-deployment-54d89d4698-85hhh --namespace=erabliere-api | grep 13ms --before-context=20 --after-context=4
```
