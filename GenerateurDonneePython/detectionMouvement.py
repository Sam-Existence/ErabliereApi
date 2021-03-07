# Source : https://opensource.com/article/20/11/motion-detection-raspberry-pi

from gpiozero import MotionSensor
import json
import requests
import pytz
import threading
import sys
from datetime import datetime as dt
from datetime import timedelta as td
from datetime import timezone
from time import sleep, time
from apscheduler.schedulers.background import BackgroundScheduler

# Raspberry Pi GPIO pin config
sensor = MotionSensor(14)

# Detection dompeux settings
threshold_seconds = 11
min_element = 5
collect = []
scheduler = BackgroundScheduler()

# Api url
urlBase = "https://erabliereapi.freddycoder.com"
if len(sys.argv) > 1:
    urlBase = sys.argv[1]

# Erabliere id
idErabliere = "1"
if len(sys.argv) > 2:
    idErabliere = sys.argv[2]

def send_data():
    # todo : le timezone dans la date est incorrect.
    print('dompeux is over, sending data...')
    url = urlBase + "/erablieres/" + idErabliere + "/dompeux"
    print(url)
    donnees = {'idErabliere': int(idErabliere),
               'dd': collect[0].isoformat(),
               'df': collect[len(collect)-1].isoformat()}
    h = {"Content-Type":"Application/json"}
    print(json.dumps(donnees))
    r = requests.post(url, json = donnees, headers = h, timeout = 2)
    print(r.status_code)
    print(r.text)
    print('done.')

def on_motion():
    de = dt.now(timezone.utc).astimezone()
    print((de - td(hours=5)).strftime("%Y-%m-%d %H:%M:%S"), 'Motion detected!')
    if len(collect) > 0 and de - collect[len(collect)-1] > td(seconds=threshold_seconds):
        print("clear", len(collect), "data.")
        collect.clear()
    collect.append(de)

def no_motion():
    print((dt.utcnow() - td(hours=5)).strftime("%Y-%m-%d %H:%M:%S"), 'nm')
    if len(collect) >= min_element:
        print("sending data in", threshold_seconds, "if no more movement")
        if len(collect) > min_element:
            try:
                scheduler.remove_job('send_dompeux')
        scheduler.add_job(send_data,
                          'date',
                          id='send_dompeux',
                          next_run_time=dt.utcnow() + td(seconds=threshold_seconds))
    else:
        print("need", min_element - len(collect), "more movement to interprete this movement as dompeux")

print('* Setting up...')

print('* Do not move, setting up the PIR sensor...')
sensor.wait_for_no_motion()

print('* Device ready! ', end='', flush=True)

sensor.when_motion = on_motion
sensor.when_no_motion = no_motion
scheduler.start()
print('Press Ctrl+C to exit\n\n')
sleep(60*24*200)
