# An docker image containing a python script to extract information from a HMI
# it is based on ubuntu and posses all teseract-ocr dependencies
FROM ubuntu:20.04
WORKDIR /app
RUN apt-get update && apt-get install -y python3-pip && apt-get install -y tesseract-ocr
COPY PythonScripts/extraireInfoImage.py /app/extraireInfoHmi.py
COPY PythonScripts/requirements.txt /app/requirements.txt
RUN pip install Pillow
RUN pip3 install -r requirements.txt