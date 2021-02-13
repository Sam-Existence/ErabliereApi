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

