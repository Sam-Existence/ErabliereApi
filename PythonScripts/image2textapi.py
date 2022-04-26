from flask import Flask
from flask import request
from base64 import b64decode
from PIL import Image
from pytesseract import image_to_string

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Welcome to the image2text api!</p><p>Send POST request to /image2text, let the body be the image in base64</p>"

@app.route("/image2text", methods = ['POST'])
def image2text():
    paramMode = request.args.get("mode")
    rawSize = request.args.get("size")
    paramSize = tuple([int(rawSize[0]), int(rawSize[1])])

    image_bytes = request.files['file'].read()
    
    image = Image.frombytes(mode = paramMode, size = paramSize, data = image_bytes)
    text = image_to_string(image)

    return text