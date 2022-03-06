from datetime import datetime as dt
from datetime import timedelta as td
from erabliere_api_proxy import ErabliereApiProxy
import requests
import os
import sys

apiKey = sys.argv[1]
locationKey = sys.argv[2]

# If the file apiKey exist, read it
if os.path.isfile(apiKey):
    with open(apiKey, 'r') as f:
        apiKey = f.read()

url = "http://dataservice.accuweather.com/currentconditions/v1/1365711?apikey=" + apiKey + "&language=fr&_=" + locationKey

print("Requesting " + url)
response = requests.get(url)

# Print information and body of the response
print(response.status_code)
print(response.headers)
print(response.text)

# Parse the json
data = response.json()

# Send the temperature to ErabliereAPI using ErabliereApiProxy
url = sys.argv[3]
idCapteur = sys.argv[4]
proxy = ErabliereApiProxy(url, "AzureAD")
print("Sending temperature to ErabliereAPI")
response = proxy.envoyer_donnee_capteur(idCapteur, int(data[0]['Temperature']['Metric']['Value'] * 10))

# Print the response
print(response.status_code)
print(response.headers)
print(response.text)

# If the program failed, return an error code
if response.status_code != 200:
    sys.exit(1)

