import datetime
import math
import sys
import json
from os import path

# x = nb heure depuis dernière gelé
def ndg(x):
  return - 0.002 * (x - 22)**2 + 1

# x = nb heure depuis dernière gelé
# t = temperature
# v = vaccium
def fdompeux(x, t, v):
  return - (ndg(x) * t * v)**2 + 5

def relu(x):
  if x > 0:
    return x
  else:
    return 0

# id = id de l'érablière
def obtenirTempsDepuisDerniereGeler(id):
  chemin = "/tmp/erabliere/donneeerabliere_" + str(id) + ".json"
  if path.exists(chemin):
    with open(chemin, 'r') as f:
      data = json.load(f)
      return data.date
  else:
    return 0

# id = id de l'érablière
# data = un objet avec comme propriété date
def sauvegarderTempsDepuisDerniereGeler(id, data):
  chemin = "/tmp/erabliere/donneeerabliere_" + str(id) + ".json"
  if path.exists(chemin):
    with open(chemin, 'w') as f:
      json.dump(data, f, sort_keys=True)

# id = id de l'érablière
# d  = data
# t  = temperature
# v  = vaccium
def calculerProgressionDompeux(id, d, t, v)
  x = obtenirTempsDepuisDerniereGeler(id)
  y = relu(fdompeux(x, t, v))
  data = {}
  data['accumulation'] += y
  sauvegarderTempsDepuisDerniereGeler(id, data)
