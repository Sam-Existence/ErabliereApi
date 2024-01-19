# ErabliereAPI

## Executer

Demande .net 8.0 SKD

## Faire confiance au certificat de dev

```
dotnet dev-certs https --trust
```

## Gestion des secrets

Lister les secrets
```
dotnet user-secrets list
```

Désactiver l'authentification
```
dotnet user-secrets set USE_AUTHENTICATION false
```