#!/bin/bash

FIVESHOME=/root/fives
WEBAPPS=/var/www/html/
SFHOME=/root/3DVS/SmartFactory

echo ${FIVESHOME}
echo ${WEBAPPS}

cp ${SFHOME}/SmartFactory.zip ${WEBAPPS}
cd ${WEBAPPS}; unzip SmartFactory.zip

