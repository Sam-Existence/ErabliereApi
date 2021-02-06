import requests
import datetime
import math
import random

def temperature(t):
  mois = t.strftime('%m')
  m = int(mois)
  return int((4.2 * math.sin((m + 1) * math.pi / 6) + 13.7) + random.triangular(-30, 30, 1))

url = "http://192.168.0.103:5000/erablieres/0/Donnees"
t = temperature(datetime.datetime.now())
donnees = {'t': t, 'nb': 50, 'v': 25, 'idErabliere': 0}
h = {"Content-Type":"Application/json"}
r = requests.post(url, json = donnees, headers = h, timeout=5)

print(r.text)
