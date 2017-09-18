#!/bin/bash

FIVESHOME=/root/fives
WEBAPPS=/var/www/html/
WPHOME=/root/3DVS/Whirlpool

echo ${FIVESHOME}
echo ${WEBAPPS}

cp -r ${WPHOME}/DeviationMaps ${FIVESHOME}/Plugins/

cp -r ${WPHOME}/WebClientWp ${WEBAPPS}

cp -f ${WPHOME}/FIVES.sln ${FIVESHOME}


