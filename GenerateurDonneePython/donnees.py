import requests
import datetime
import math
import random
import sys

def temperature(t):
  mois = t.strftime('%m')
  m = int(mois)
  doy = t.timetuple().tm_yday
  hod = t.timetuple()[3]
  return int((12 * math.sin((doy-105)*math.pi/182.5)) + (5*math.sin(doy*4)) + (5 * math.sin((hod - 7) * 3.14159 / 12)))

def getVaccium(id, t):
  vaccium = 0
  if t >= -1 - id:
    vaccium = 24
  if t > 3:
    vaccium = + 1
  return vaccium

def getNiveauBassin():
  niveauBassin = 0
  return niveauBassin

nbErabliere = 1
if len(sys.argv) > 1:
  nbErabliere = int(sys.argv[1])

urlBase = "https://erabliereapi.freddycoder.com"
if len(sys.argv) > 2:
  urlBase = sys.argv[2]

for id in range(0, nbErabliere):
  print("Érablière :", id)
  url = urlBase + "/erablieres/" + str(id) + "/Donnees"
  t = temperature(datetime.datetime.now()) + id
  vaccium = 0
  print("La temperature est", t)
  if t >= -2 + id:
    vaccium = getVaccium(id, t)
  print("Le vaccium est", vaccium)
  donnees = {'t': t, 'nb': 0, 'v': vaccium, 'idErabliere': id}
  h = {"Content-Type":"Application/json"}
  r = requests.post(url, json = donnees, headers = h, timeout = 2)

print(r.text)
