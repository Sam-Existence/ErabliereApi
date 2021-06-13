import requests
import datetime
import math
import sys
from auth.getAccessToken import getAccessToken

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

token = getAccessToken("https://192.168.0.103:5005/connect/token", "raspberrylocal", "secret")

for id in range(1, nbErabliere + 1):
  print("Érablière :", id)
  url = urlBase + "/erablieres/" + str(id) + "/Donnees"
  t = temperature(datetime.datetime.utcnow()) + (id*10)
  vaccium = 0
  print("La temperature est", t/10)
  if t >= -2 + id:
    vaccium = getVaccium(id, t)
  print("Le vaccium est", vaccium)
  donnees = {'t': t, 'nb': 0, 'v': vaccium, 'idErabliere': id}
  h = {"Authorization": "Bearer " + token, "Content-Type":"Application/json"}
  r = requests.post(url, json = donnees, headers = h, timeout = 2, verify = False)
  print(r.text)
