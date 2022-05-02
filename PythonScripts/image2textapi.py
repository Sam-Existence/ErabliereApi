from flask import Flask
from flask import request
from base64 import b64decode
from PIL import Image
from pytesseract import image_to_string
import uuid
import os

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Welcome to the image2text api!</p><p>Send POST request to /image2text with the images in the body of the request</p>"

@app.route("/image2text", methods = ['POST'])
def image2text():
    image_bytes = request.files['file'].read()

    temp_file = "/tmp/tmp_image" + str(uuid.uuid1()) + ".jpg"

    # Check if it is windows
    if os.name == 'nt':
        temp_file = "__pycache__\\tmp_image.jpg"

    file = open(temp_file, "wb")
    print("write file", temp_file)
    file.write(image_bytes)

    file.close()

    image = Image.open(temp_file)
    text = image_to_string(image)

    os.remove(temp_file)

    return text

@app.route("/logs")
def get_logs():
    with open('/var/log/image2text.log') as f:
        lines = f.read()
    return '<span style="white-space: pre-line">' + lines + '</span>'