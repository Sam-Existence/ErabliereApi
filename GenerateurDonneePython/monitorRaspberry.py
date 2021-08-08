# sudo apt install libraspberrypi-bin

# available command
#vcgencmd measure_temp
#vcgencmd get_throttled
#vcgencmd measure_volts
#vcgencmd get_mem arm
#vcgencmd get_mem gpu

# free -mh

# inspiration
# https://www.cloudsavvyit.com/9996/monitoring-temperature-on-the-raspberry-pi/
# https://medium.com/@kevalpatel2106/monitor-the-core-temperature-of-your-raspberry-pi-3ddfdf82989f

# To make the script work you may need to do
# sudo usermod -aG video <username> (source: https://chewett.co.uk/blog/258/vchi-initialization-failed-raspberry-pi-fixed/)
# and reboot the PI

import os
import time
import sys

def measure_temp():
        temp = os.popen("vcgencmd measure_temp").readline()
        return (temp.replace("temp=",""))


t = measure_temp()

print(t)

if len(sys.argv) == 1:
  exit()

from erabliere_api_proxy import ErabliereApiProxy

authType = None

if len(sys.argv) > 2:
  authType = sys.argv[2]

proxy = ErabliereApiProxy(sys.argv[1], authType)

def getAllDigit(x):
  xp = ""
  for c in x:
    if c.isdigit():
      xp = xp + c

  return int(xp)

r = proxy.envoyer_donnee_capteur(int(sys.argv[len(sys.argv)-1]), getAllDigit(t))

print(r)
