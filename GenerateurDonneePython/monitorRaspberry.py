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

def measure_temp():
        temp = os.popen("vcgencmd measure_temp").readline()
        return (temp.replace("temp=",""))


print(measure_temp())
