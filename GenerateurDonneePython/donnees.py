import requests
import datetime
import math
import random

def temperature(t):
  mois = t.strftime('%m')
  m = int(mois)
  doy = t.timetuple().tm_yday
  hod = t.timetuple()[3]
  return int((12 * math.sin((doy-105)*math.pi/182.5)) + (5*math.sin(doy*4)) + (5 * math.sin((hod - 7) * 3.14159 / 12)))

url = "http://192.168.0.103:5000/erablieres/0/Donnees"
t = temperature(datetime.datetime.now())
donnees = {'t': t, 'nb': 0, 'v': 0, 'idErabliere': 0}
h = {"Content-Type":"Application/json"}
r = requests.post(url, json = donnees, headers = h, timeout=5)

print(r.text)
