## donnes.py

script de génération de données utiliser pour facilité le développement

## detectionMouvement.py

script pour interagir avec un capteur détecteur de mouvement pour capturer les dompeux lors de la saison des sucres.

Les scripts suivant sont executer sur un raspberry pi 3 qui execute ubuntu server comme OS. 

```bash
sudo apt update
sudo apt install python3-gpiozero
sudo apt install python3-pip
sudo pip3 install apscheduler
sudo pip3 install pytz
```

documentation apscheduler : https://apscheduler.readthedocs.io/en/stable/modules/schedulers/base.html#apscheduler.schedulers.base.BaseScheduler.add_job

### Lancer le script au démarrage du raspberry

Copier le script dans le repertoire /bin
```
sudo cp -i detectionMouvement.py /bin
sudo crontab -e
@reboot python3 /bin/detectionMouvement.py >/var/log/detectionMouvement.log 2>&1
```

Capturer les logs de cron
```
sudo nano /etc/rsyslog.d/50-default.conf
# Enlever le # devant cron.*
sudo service rsyslog restart
service cron restart
cat /var/log/cron.log
sudo halt --reboot
```

## extraireInfoImage.py

Script pour extraire des informations d'une image. Le but est d'extraire les information d'un paneau d'un interface HMI avec la fonctionnalité server web activé. Il est possible de faire une requête au panneau pour obtenir l'écran demander sous image jpg.

```
python3 extraireInfoImage.py https://www.acscm.com/wp-content/uploads/images/news/2015/5-tips-for-better-hmi-page.png
```

### Cronjob

```
# Au 5 minute                                                                             adresse panneau hmi   adresse api action post données                           id erabliere
*/5 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/extraireInfoImage.py http://<ip-hmi>/1.jpg https://erabliereapi.freddycoder.com/erablieres/3/donnees 3
```

## modbus-dl06

Script pour communiquer avec un PLC Direct Logic 06 utilisant modbustcp.

Prérequis
```
sudo pip3 install pymodbus
```

source : https://pymodbus.readthedocs.io/en/latest/readme.html#summary