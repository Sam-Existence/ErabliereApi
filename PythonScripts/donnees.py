import requests
import datetime
import json
import math
import sys
from erabliere_api_proxy import ErabliereApiProxy

#parse required argument idErabliere and urlBase
idErabliere = sys.argv[1]
urlBase = sys.argv[2]

def temperature(t):
  mois = t.strftime('%m')
  m = int(mois)
  doy = t.timetuple().tm_yday
  hod = t.timetuple()[3]
  return int((12 * math.sin((doy-105)*math.pi/182.5)) + (5*math.sin(doy*4)) + (5 * math.sin((hod - 7) * 3.14159 / 12)) * 10)

def getVaccium(t):
  vaccium = 0
  if t >= -1:
    vaccium = 24.1
  if t > 3:
    vaccium += 1
  return int(vaccium * 10)

def getNiveauBassin():
  niveauBassin = 0
  return niveauBassin

proxy = ErabliereApiProxy(urlBase, "AzureAD")

print("Érablière :", idErabliere)
t = temperature(datetime.datetime.utcnow())
vaccium = 0
print("La temperature est", t/10)
if t >= -2:
  vaccium = getVaccium(t)
print("Le vaccium est", vaccium)
r = proxy.envoyer_donnees(idErabliere, t, vaccium, niveaubassin = 0)
print(r)
print(r.text)