# Archives

Documentation précédentes pour références si nécessaire.

# GenerateurDonneePython

Dossier contenant des scripts permettant d'envoyer des données à l'api. Les script sont lancer avec cron. Pour obtenir des logs plus détaillé, voir le bout de code bash suivant :

Capturer les logs de cron
```
sudo nano /etc/rsyslog.d/50-default.conf
# Enlever le # devant cron.*
sudo service rsyslog restart
service cron restart
cat /var/log/cron.log
sudo halt --reboot
```

## detectionMouvement.py

Script pour interagir avec un capteur détecteur de mouvement pour capturer les dompeux lors de la saison des sucres. L'idée était de placer un capteur près de la coulé d'eau dans un bassin, mais le capteur de détecte pas bien ce type de mouvement.

Les scripts suivant sont executer sur un raspberry pi 3 qui execute ubuntu server comme OS. 

```bash
sudo apt update
sudo apt install python3-gpiozero
sudo apt install python3-pip
sudo pip3 install apscheduler
sudo pip3 install pytz
```

### Lancer le script au démarrage du raspberry

Copier le script dans le repertoire /bin
```
sudo cp -i detectionMouvement.py /bin
sudo crontab -e
@reboot python3 /bin/detectionMouvement.py >/var/log/detectionMouvement.log 2>&1
```

## dl06_modbus.py

Script permettant de scanner une série de registre du PLC et d'envoyer les valeurs à l'api

### Cronjob

L'url de l'api est hard-codé dans le script.

```
*/15 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/dl06_modbus_client.py >/home/ubuntu/dl06_modbusextract_run.log 2>&1
```

## donnes.py

script de génération de données utiliser pour facilité le développement

documentation apscheduler : https://apscheduler.readthedocs.io/en/stable/modules/schedulers/base.html#apscheduler.schedulers.base.BaseScheduler.add_job

### Cronjob

Envoyer des donnée a un api sur une machine de développement

```
*/1 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/donnees.py 3 http://192.168.0.103:5000 >/home/ubuntu/generateurdonnees_debug.log 2>&1
```

## extraireInfoImage.py

Script pour extraire des informations d'une image. Le but est d'extraire les information d'un paneau d'un interface HMI avec la fonctionnalité server web activé. Il est possible de faire une requête au panneau pour obtenir l'écran demander sous image jpg.

```
python3 extraireInfoImage.py https://www.acscm.com/wp-content/uploads/images/news/2015/5-tips-for-better-hmi-page.png
```

### Cronjob

```
# Au 5 minute                                                                             adresse panneau hmi   adresse api action post données                           id erabliere
*/5 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/extraireInfoImage.py http://<ip-hmi>/1.jpg https://erabliereapi.freddycoder.com <guid-id-erabliere>
```

## monitorRaspberry

Un script permettant de lire la temperature du processeur du raspberry et d'envoyer la valeur à l'api.

### Cronjob

```
*/5 * * * * python3 /home/ubuntu/erabliereapi/GenerateurDonneePython/monitorRaspberry.py https://erabliereapi.freddycoder.com AzureAD <guid-id-erabliere> >/home/ubuntu/monitor_raspberry.log 2>&1
```
