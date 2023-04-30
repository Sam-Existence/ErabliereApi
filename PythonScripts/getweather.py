from erabliere_api_proxy import ErabliereApiProxy
import requests
import os
import sys

apiKeyFilePath = sys.argv[1]
locationKey = sys.argv[2]

# Validate against path traversal attack

# If the file apiKey exist, read it
if os.path.isfile(apiKeyFilePath):
    with open(apiKeyFilePath, 'r') as f:
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
response = proxy.envoyer_donnee_capteur(idCapteur, int(data[0]["Temperature"]["Metric"]["Value"] * 10), data[0]["WeatherText"])

# Print the response
print(response.status_code)
print(response.headers)
print(response.text)

# If the program failed, return an error code
if response.status_code != 200:
    sys.exit(1)

