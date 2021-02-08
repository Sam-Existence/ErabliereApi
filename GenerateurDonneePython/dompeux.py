import requests
import datetime
import math
import random

url = "https://erabliereapi.freddycoder.com/erablieres/0/Donnees"

def obtenirDompeuxPlusRecent(id):
  reponse = requests.get("https://erabliereapi.freddycoder.com/erablieres/" + str(id) + "/Dompeux?q=1&o=c")
  print("Dernier dompeux erabliere", id, reponse.text)

obtenirDompeuxPlusRecent(0)

#h = {"Content-Type":"Application/json"}
#dompeux = {'t': datetime.datetime.now(), 'idErabliere': 0}
#r = requests.post(url, json = dompeux, headers = h, timeout=5)
#print(r.text)
