# PythonScripts

Les scripts python sont installé sur des ordinateurs permettant d'interoger des appareils qui n'ont pas la capacité d'envoyer des requêtes http.

### Installer les dépendances

Sous windows:
```
pip install -r requirements.txt
```

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

## extraireInfoHmi.py

```
python .\extraireInfoHmi.py http://<ip-address-hmi>/1.jpg https://erabliereapi.freddycoder.com <guid-erabliere>
```

Version plus complexe envoyant les données récupéré à deux API une sans vérification ssl et l'autre avec les vérification ssl.

```
python3 /home/ubuntu/erabliereapi/PythonScripts/extraireInfoHmi.py http:/<ip-address-hmi>/1.jpg [noSslVerify]https://192.168.1.2:5001,https://erabliereapi.freddycoder.com <guid-erabliere>
```

## getwather.py

```
python3 getweather.py <acuweather-api-key-file-path> <location> <api-domain> <capteur-guid-id>
```
ex:
```
python3 getweather.py /home/ubuntu/.erabliereapi/acuweather.key 1365711 https://erabliereapi.freddycoder.com 00000000-0000-0000-0000-000000000000
```

## image2textapi.py

Documentation à venir

```python

```