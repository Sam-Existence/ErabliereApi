import json
import requests
import os
from urllib.parse import urlparse
from auth.getAccessToken import getAccessToken as getAccessTokenIdentity
from auth.getAccessTokenAAD import getAccessToken as getAccessTokenAAD
from auth.getAccessTokenAAD import AzureADAccessTokenProvider

class ErabliereApiProxy:
  def __init__(self, baseUrl, auth_provider = None, verifySsl = True):
    self.baseUrl = baseUrl
    self.auth_provider = auth_provider
    self.aad_token_provider = AzureADAccessTokenProvider()
    self.authConfig = None
    self.verifySsl = verifySsl
    self.host = urlparse(baseUrl).netloc
    self.timeout = 5

  def get_donnees(self, id_erabliere: str, q: int, o: str):
    data = self.get_request("/erablieres/" + str(id_erabliere) + "/donnees?q=" + str(q) + "&o=" + o)
    return data.json()

  def envoyer_donnees(self, id_erabliere: str, temperature: int, vaccium: int, niveaubassin: int):
    donnee = {'t': temperature, 'nb': niveaubassin, 'v': vaccium, 'idErabliere': id_erabliere}
    return self.post_request("/erablieres/" + str(id_erabliere) + "/donnees", donnee)

  def envoyer_dompeux(self, id_erabliere, datedebut, datefin):
    dompeux = {'idErabliere': int(id_erabliere),
               'dd': datedebut,
               'df': datefin}
    return self.post_request("/erablieres/" + str(id_erabliere) + "/dompeux", dompeux)

  def envoyer_donnee_capteur(self, id_capteur, valeur, text = None):
    donnee = {'V': valeur, 'idCapteur': id_capteur, 'text': text}
    return self.post_request("/capteurs/" + str(id_capteur) + "/donneesCapteur", donnee)

  def creer_capteur(self, id_erabliere, nom, symbole, afficherCapteurDashboard):
    capteur = {'nom': nom, 'symbole': symbole, 'afficherCapteurDashboard': afficherCapteurDashboard, 'idErabliere': id_erabliere }
    return self.post_request("/erablieres/" + str(id_erabliere) + "/capteurs", capteur)

  def get_request(self, action) -> requests.Response:
    token = self.get_token()
    h = {"Authorization": "Bearer " + str(token)}
    r = requests.get(self.baseUrl + action, headers = h, timeout = self.timeout, verify = self.verifySsl)
    return r

  def post_request(self, action, body) -> requests.Response:
    token = self.get_token()
    h = {"Authorization": "Bearer " + str(token), "Content-Type":"Application/json"}
    r = requests.post(self.baseUrl + action, json = body, headers = h, timeout = self.timeout, verify = self.verifySsl)
    return r

  def get_token(self):
    if (self.auth_provider == None or self.auth_provider == "None"):
      return None
    if (self.auth_provider == "Identity"):
      return getAccessTokenIdentity("https://192.168.0.103:5005/connect/token", "raspberrylocal", "secret")
    if (self.auth_provider == "AzureAD"):
      if self.authConfig == None:
        authPath = f"/home/ubuntu/.erabliereapi/auth.{self.host}.config"
        if (os.name == "nt"):
          authPath = f"E:\\config\\python\\aad-client-credentials.{self.host}.json"
        print("Open config from file", authPath)
        authConfig = open(authPath,)
        self.authConfig = json.load(authConfig)[0]
        authConfig.close()
      token = self.aad_token_provider.getAccessToken(self.authConfig)
      return token

    raise NameError("The name of the auth_provider is invalid. Must be None, 'Identity' or 'AzureAD'. The value was " + str(self.auth_provider) + ".")

