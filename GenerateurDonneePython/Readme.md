## Programmes requis

Les scripts suivant sont executer sur un raspberry pi 3 qui execute ubuntu server comme OS. 

```bash
sudo apt update
sudo apt install python3-gpiozero
sudo apt install python3-pip
pip3 install apscheduler
```

## donnes.py

script de génération de données utiliser pour facilité le développement

## dompeux.py

ébauche d'un script pour générer des faux dompeux, n'est pas utilisé.

## detectionMouvement.py

script pour interagir avec un capteur détecteur de mouvement pour capturer les dompeux lors de la saison des sucres.

documentation apscheduler : https://apscheduler.readthedocs.io/en/stable/modules/schedulers/base.html#apscheduler.schedulers.base.BaseScheduler.add_job
