# An docker image containing a python script to extract information from a HMI
# it is based on ubuntu and posses all teseract-ocr dependencies
FROM ubuntu:latest
WORKDIR /app
ENV TZ=Canada/Eastern
ENV FLASK_APP=image2textapi
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN apt-get update && apt-get install -y python3-pip && apt-get install -y tesseract-ocr
COPY PythonScripts/. /app/.
RUN pip install Pillow
RUN pip3 install -r requirements.txt
USER 1000:1000