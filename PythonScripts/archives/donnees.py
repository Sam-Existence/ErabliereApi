import requests
import datetime
import json
import math
import sys
from erabliere_api_proxy import ErabliereApiProxy

def temperature(t):
  mois = t.strftime('%m')
  m = int(mois)
  doy = t.timetuple().tm_yday
  hod = t.timetuple()[3]
  return int((12 * math.sin((doy-105)*math.pi/182.5)) + (5*math.sin(doy*4)) + (5 * math.sin((hod - 7) * 3.14159 / 12)) * 10)

def getVaccium(id, t):
  vaccium = 0
  if t >= -1 - id:
    vaccium = 24.1
  if t > 3:
    vaccium += 1
  return int(vaccium * 10)

def getNiveauBassin():
  niveauBassin = 0
  return niveauBassin

nbErabliere = 1
if len(sys.argv) > 1:
  nbErabliere = int(sys.argv[1])

urlBase = "https://erabliereapi.freddycoder.com"
if len(sys.argv) > 2:
  urlBase = sys.argv[2]

proxy = ErabliereApiProxy(urlBase, None)
if urlBase != "http://192.168.0.103:5000":
  proxy = ErabliereApiProxy(urlBase, "AzureAD")

for id in range(1, nbErabliere + 1):
  print("Érablière :", id)
  t = temperature(datetime.datetime.utcnow()) + (id*10)
  vaccium = 0
  print("La temperature est", t/10)
  if t >= -2 + id:
    vaccium = getVaccium(id, t)
  print("Le vaccium est", vaccium)
  r = proxy.envoyer_donnees(id, t, vaccium, niveaubassin = 0)
  print(r)
  print(r.text)
