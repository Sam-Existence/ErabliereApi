# Source : https://opensource.com/article/20/11/motion-detection-raspberry-pi

from gpiozero import MotionSensor
import threading
from datetime import datetime as dt

# Raspberry Pi GPIO pin config
sensor = MotionSensor(14)

def on_motion():
    print(dt.now().strftime("%H:%M:%S"), 'Motion detected!')

def no_motion():
    print(dt.now().strftime("%H:%M:%S"), 'No motion')

print('* Setting up...')

print('* Do not move, setting up the PIR sensor...')
sensor.wait_for_no_motion()

print('* Device ready! ', end='', flush=True)

sensor.when_motion = on_motion
sensor.when_no_motion = no_motion
input('Press Enter or Ctrl+C to exit\n\n')
