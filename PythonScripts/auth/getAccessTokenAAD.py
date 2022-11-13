# sudo pip3 install azure-identity
from azure.identity import ClientSecretCredential
import json
from os import path
import os
import time
from urllib.parse import urlparse

def getAccessToken(config):
  print("Get AAD access token")
  aadTokenFile = '/home/ubuntu/aad_oauth_token.json'
  if os.name == 'nt':
    aadTokenFile = '__pycache__\\aad_oauth_token.json'
  if path.exists(aadTokenFile):
    f = open(aadTokenFile,)
    data = json.load(f)
    expireOn = data[1]
    if int(time.time()) < expireOn:
      print('Reuse existing token')
      print("Access token lifetime: " + str(data[1] - int(time.time())))
      return data[0]

  print("Get token from AzureAD")
  client = ClientSecretCredential(config["TenantId"], config["ClientId"], config["ClientSecret"], authority = config["Authority"])

  accessToken = client.get_token(config["Scopes"])

  print("Access token lifetime: " + str(accessToken[1] - int(time.time())))

  pathSaveFile = '/home/ubuntu/aad_oauth_token.json'
  if os.name == 'nt':
    pathSaveFile = '__pycache__\\aad_oauth_token.json'
  with open(pathSaveFile, 'w') as outfile:
    json.dump(accessToken, outfile)

  return accessToken.token

class AzureADAccessTokenProvider:
  def __init__(self):
    self.in_memory_token = None

  def getAccessToken(self, config):
    print("Get AAD access token")

    if (self.in_memory_token != None and int(time.time()) < self.in_memory_token[1]):
      print("Return token from memory")
      print("Access token lifetime: " + str(self.in_memory_token[1] - int(time.time())))
      return self.in_memory_token[0]

    aadTokenFile = f'/home/ubuntu/aad_oauth_token.{urlparse(config["Scopes"]).netloc}.json'
    if os.name == 'nt':
      aadTokenFile = f'__pycache__\\aad_oauth_token.{urlparse(config["Scopes"]).netloc}.json'
    if path.exists(aadTokenFile):
      f = open(aadTokenFile,)
      self.in_memory_token = json.load(f)
      expireOn = self.in_memory_token[1]
      if int(time.time()) < expireOn:
        print('Reuse existing token')
        print("Access token lifetime: " + str(self.in_memory_token[1] - int(time.time())))
        return self.in_memory_token[0]

    print("Get token from AzureAD")
    client = ClientSecretCredential(config["TenantId"], config["ClientId"], config["ClientSecret"], authority = config["Authority"])

    accessToken = client.get_token(config["Scopes"])

    print("Access token lifetime: " + str(accessToken[1] - int(time.time())))

    try:
      pathSaveFile = f'/home/ubuntu/aad_oauth_token.{urlparse(config["Scopes"]).netloc}.json'
      if os.name == 'nt':
        pathSaveFile = f'__pycache__\\aad_oauth_token.{urlparse(config["Scopes"]).netloc}.json'
      with open(pathSaveFile, 'w') as outfile:
        json.dump(accessToken, outfile)
    except:
      print("Error saving token to file")

    self.in_memory_token = accessToken

    return accessToken.token

