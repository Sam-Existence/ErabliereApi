# ErabliereIU

> This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 11.0.7.

Projet contenant le projet d'interface graphique de ErabliereAPI.

## Installer les dépendances

Il faut avoir node js d'installé sur votre poste. https://nodejs.org

Le cli angular doit aussi être installé.

```
npm install -g @angular/cli
```

Une fois les prérequis installé:

```
npm install
```

## Executer le projet avec npm

```
npm start
```

## Utiliser les mecanismes d'optimisation en développement

L'api utilise des entêtes http pour optimiser la quantitée de données échanger. Les politiques CORS empêche les entêtes d'être lu par le navigateur lorque là requête vient d'un autre origine. Pour tester les mecanismes en développement, executer la commande suivante :

```
ng build --output-path="..\ErabliereApi\wwwroot\."
```

Démarrer le projet ErabliereApi.

Pour plus d'info sur les entêtes, rechercher ```x-ddr``` et ```x-dde``` dans le repository ou consulter la page <a href="https://erabliereapi.freddycoder.com/api/index.html" tagert="_blank">swagger</a> pour les actions GET Dompeux et GET Donnees.

## Créer un certificat ssl

https://medium.com/@richardr39/using-angular-cli-to-serve-over-https-locally-70dab07417c8

## Mettre a jour le projet et ses dépendances

```
ng update
```