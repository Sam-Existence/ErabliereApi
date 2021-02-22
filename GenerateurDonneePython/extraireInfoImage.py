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

import requests
import sys

print("requests", sys.argv[1])
response = requests.get(sys.argv[1])
print(response.status_code)
temp_file = "/tmp/panel_erabliere_saint_alfred.jpg"
file = open(temp_file, "wb")
print("write file", temp_file)
file.write(response.content)

file.close()

print("load image modules")
from PIL import Image
import os, re
from pytesseract import image_to_string

print("open image", temp_file)

img = Image.open("/tmp/panel_erabliere_saint_alfred.jpg")

print("image to text...")
text = image_to_string(img)

print(text)

r_temperature = re.compile("-*\d+\.\d\s*[°C]")

print("temperature", r_temperature.match(text))

r_vaccium = re.compile("-*\d+\.\d\s*[HG]")

print("vaccium", r_vaccium.match(text))

r_niveaubassin = re.compile("-*\d+\.\d\s*[%]")

print("niveau bassin", r_niveaubassin.match(text))


