import requests
import datetime
import math
import random
import system

nbErabliere = 1
if len(sys.argv) > 1:
  nbErabliere = int(sys.argv[1])

urlBase = "https://erabliereapi.freddycoder.com"
if len(sys.argv) > 2:
  urlBase = sys.argv[2]

url = urlBase + "/erablieres/0/Donnees"

def obtenirDompeuxPlusRecent(id):
  reponse = requests.get(url + "/erablieres/" + str(id) + "/Dompeux?q=1&o=c")
  print("Dernier dompeux erabliere", id, reponse.text)

for i in range(nbErabliere):
  obtenirDompeuxPlusRecent(i)
  
