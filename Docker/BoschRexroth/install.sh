#!/bin/bash

FIVESHOME=/root/fives
WEBAPPS=/var/www/html/
BRHOME=/root/3DVS/BoschRexroth

echo ${FIVESHOME}
echo ${WEBAPPS}

cp -r ${BRHOME}/CustomizedAnimation ${FIVESHOME}/Plugins/
cp -r ${BRHOME}/SceneParser ${FIVESHOME}/Plugins/

cp ${BRHOME}/WebClientBR.zip ${WEBAPPS}
cd ${WEBAPPS}; unzip WebClientBR.zip

cp -f ${BPHOME}/FIVES.sln ${FIVESHOME}


