# PythonScripts

Les scripts python sont installé sur des serveurs permettant d'interoger des appareils qui n'ont pas la capacité d'envoyer des requêtes http.

### Authentification

Trois mode d'authentification sont supporté (Aucune authentification, IdentityServer4 et AzureAD). La documentation suivante se concentrera sur l'installation des scripts avec AzureAD.

Les configurations d'authentification en mode 'client credentials' devront se trouvé dans

Linux
```
/home/ubuntu/.erabliereapi/auth.config
```

Windows
```
E:\config\python\aad-client-credentials.json
```

Le json de configuration devra ressembler à ceci:


```
[
    {
        "TenantId": "<tenantId>",
        "ClientId": "<clientId>",
        "ClientSecret": "<clientSecret>",
        "Authority": "https://login.microsoftonline.com",
        "Scopes": "api://<api-clientId>/.default"
    }
]
```

### Dépendances

Les dépendances des scripts sont restauré utilisant pip et le fichier requirements.txt

```
pip install -r requirements.txt
```

## extraireInfoImage.py

```
python .\extraireInfoImage.py http://<ip-address-hmi>/1.jpg https://erabliereapi.freddycoder.com <guid-erabliere>
```

## Work in progress

Les autres scripts et la documentation est en cours d'amélioration.