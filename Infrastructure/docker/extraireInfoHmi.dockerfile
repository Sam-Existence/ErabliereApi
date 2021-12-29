# An docker image containing a python script to extract information from a HMI
# it is based on ubuntu and posses all teseract-ocr dependencies
FROM ubuntu:20.04
WORKDIR /app
RUN apt-get update && apt-get install -y tesseract-ocr
COPY PythonScripts/extraireInfoImage.py /app/extraireInfoHmi.py
COPY PythonScripts/requirements.txt /app/requirements.txt
RUN pip install -r requirements.txt