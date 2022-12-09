# For linux
# sudo apt-get update
# sudo apt install tesseract-ocr
# sudo apt-get install python-dev python-setuptools
# sudo apt-get install libjpeg-dev -y
# sudo apt-get install zlib1g-dev -y
# sudo apt-get install libfreetype6-dev -y
# sudo apt-get install liblcms1-dev -y
# sudo apt-get install libopenjp2-7 -y
# sudo apt-get install libtiff5 -y
# sudo pip3 install pytesseract
# sudo pip3 install pillow

# For windows
# Add the tesseact-orc package in the environment variable PATH 
# the location is C:\Users\<USER>\AppData\Local\Tesseract-OCR\

from datetime import datetime as dt
from datetime import timedelta as td
from erabliere_api_proxy import ErabliereApiProxy
import requests
import os
import sys

print("Debut execution", (dt.utcnow() - td(hours=5)).strftime("%m/%d/%Y, %H:%M:%S"))

print("requests", sys.argv[1])
response = requests.get(sys.argv[1])
print(response.status_code)
temp_file = "/tmp/tmp_image.jpg"

# Check if it is windows
if os.name == 'nt':
    temp_file = "__pycache__\\tmp_image.jpg"

file = open(temp_file, "wb")
print("write file", temp_file)
file.write(response.content)

file.close()

print("load image modules")
from PIL import Image
import os, re
from pytesseract import image_to_string

print("open image", temp_file)

img = Image.open(temp_file)

print("image to text...")
text = image_to_string(img)

print(text)

r_temperature = re.findall(r"\-?\d+\.\d*", text)

print("temperature", r_temperature)

r_vaccium = re.findall(r"-*\d+\.\d\s*HG", text)

print("vaccium", r_vaccium)

r_niveaubassin = re.findall("HG \d+", text)

print("niveau bassin", r_niveaubassin)

vaccium = 0
# Cas spéciaux de image_to_string
if len(r_vaccium) > 0:
  vaccium = int(float(r_vaccium[0].replace("HG", "")) * 10)
  if vaccium > 1000:
    vaccium -= 1000

nb = 0
if len(r_niveaubassin) > 0:
  nb = int(float(r_niveaubassin[0].replace("HG ", "")))

# Envoie des données aux apis
print("Envoie des données aux adresses suivantes:", sys.argv[2])

urls = sys.argv[2]
idErabliere = sys.argv[3]

def getDate():
  return (dt.utcnow() - td(hours=5)).strftime("%m/%d/%Y, %H:%M:%S")

for url in urls.split(","):
  try:
    print(getDate(), 'POST to', url)
    
    sslVerify = True
    if url.startswith('[noSslVerify]'):
      url = url.replace('[noSslVerify]', '')
      sslVerify = False
    
    proxy = ErabliereApiProxy(url, "AzureAD", verifySsl=sslVerify)
    temperature = int(float(r_temperature[0].replace("°C", "")) * 10)
    
    if temperature == 77:
      print('Cas spéciaux de image_to_string')
      print('Vaider que la temperature précédente est negative')
      temp_precedent = proxy.get_donnees(idErabliere, q=1, o='d')
      if len(temp_precedent) == 1 and temp_precedent[0]['t'] < 0:
        temperature = -77

    reponse = proxy.envoyer_donnees(idErabliere, temperature, vaccium, nb)
    print("réponse", reponse.status_code)
    print(getDate(), "terminé.")
  except Exception as e:
    print("erreur", e)