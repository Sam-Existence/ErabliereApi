import requests
import base64
import json

def getAccessToken(url, clientId, clientSecret):
    url = "https://localhost:5005/connect/token"

    # Générer l'entête en base 64
    sample_string = clientId + ":" + clientSecret
    sample_string_bytes = sample_string.encode("ascii")
    base64_bytes = base64.b64encode(sample_string_bytes)
    base64_string = base64_bytes.decode("ascii")

    # Generer les éléments de la requête
    body = "grant_type=client_credentials&scope=erabliereapi"
    headers = {
        'Content-Type': "application/x-www-form-urlencoded",
        'Authorization': "Basic " + base64_string
        }

    # Envoyer la requete
    response = requests.request("POST", url, data=body, headers=headers, verify = False)

    responseObj = json.loads(response.text)

    return responseObj.access_token