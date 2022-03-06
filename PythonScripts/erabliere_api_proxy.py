import json
import requests
import os
from auth.getAccessToken import getAccessToken as getAccessTokenIdentity
from auth.getAccessTokenAAD import getAccessToken as getAccessTokenAAD
from auth.getAccessTokenAAD import AzureADAccessTokenProvider

class ErabliereApiProxy:
  def __init__(self, baseUrl, auth_provider = None):
    self.baseUrl = baseUrl
    self.auth_provider = auth_provider
    self.aad_token_provider = AzureADAccessTokenProvider()
    self.authConfig = None

  def envoyer_donnees(self, id_erabliere, temperature, vaccium, niveaubassin):
    donnee = {'t': temperature, 'nb': niveaubassin, 'v': vaccium, 'idErabliere': id_erabliere}
    return self.post_request("/erablieres/" + str(id_erabliere) + "/donnees", donnee)

  def envoyer_dompeux(self, id_erabliere, datedebut, datefin):
    dompeux = {'idErabliere': int(id_erabliere),
               'dd': datedebut,
               'df': datefin}
    return self.post_request("/erablieres/" + str(id_erabliere) + "/dompeux", dompeux)

  def envoyer_donnee_capteur(self, id_capteur, valeur):
    donnee = {'V': valeur, 'idCapteur': id_capteur}
    return self.post_request("/capteurs/" + str(id_capteur) + "/donneesCapteur", donnee)

  def creer_capteur(self, id_erabliere, nom, symbole, afficherCapteurDashboard):
    capteur = {'nom': nom, 'symbole': symbole, 'afficherCapteurDashboard': afficherCapteurDashboard, 'idErabliere': id_erabliere }
    return self.post_request("/erablieres/" + str(id_erabliere) + "/capteurs", capteur)

  def post_request(self, action, body):
    token = self.get_token()
    h = {"Authorization": "Bearer " + str(token), "Content-Type":"Application/json"}
    r = requests.post(self.baseUrl + action, json = body, headers = h, timeout = 5)
    return r

  def get_token(self):
    if (self.auth_provider == None or self.auth_provider == "None"):
      return None
    if (self.auth_provider == "Identity"):
      return getAccessTokenIdentity("https://192.168.0.103:5005/connect/token", "raspberrylocal", "secret")
    if (self.auth_provider == "AzureAD"):
      if self.authConfig == None:
        print("Open config from file")
        authPath = "/home/ubuntu/.erabliereapi/auth.config"
        if (os.name == "nt"):
          authPath = "E:\\config\\python\\aad-client-credentials.json"
        authConfig = open(authPath,)
        self.authConfig = json.load(authConfig)[0]
        authConfig.close()
      token = self.aad_token_provider.getAccessToken(self.authConfig)
      return token

    raise NameError("The name of the auth_provider is invalid. Must be None, 'Identity' or 'AzureAD'. The value was " + str(self.auth_provider) + ".")

