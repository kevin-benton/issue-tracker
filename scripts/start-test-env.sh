#!/bin/bash

echo "Setting up..."
PWD=`pwd`
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

cd $SCRIPT_DIR/..

export IPADDR="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

echo "Starting test environment in background..."
docker compose --file docker-compose.test.yml up -d --force-recreate --remove-orphans

if [ "$?" -ne "0" ]; then
    exit 1
fi

echo "Waiting for environment to be ready...."
started=0
while [ "$started" -eq "0" ];
do
    logs=`docker logs familyscreening_cosmos_1`
    if [[ "$logs" == *"Started"* ]]; then
        echo "Cosmos container ready for requests."
        started=1
    else
        sleep 2
    fi
done

echo "Setting local SSL certificate. This may ask for your admin password."
echo "This is currently only scripted for MAC. To support other platforms, you may need to set the SSL cert yourself."
validCert=0
while [ "$validCert" -eq "0" ];
do
    rm emulatorcert.crt
    curl -k https://localhost:8081/_explorer/emulator.pem > emulatorcert.crt
    contents=`cat emulatorcert.crt`
    if [[ "$contents" == *"BEGIN CERTIFICATE"* ]]; then
        validCert=1
    else
        sleep 2
    fi
done

sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain emulatorcert.crt

echo "Cleaning up environment."
cd $PWD

echo "Your test environment is ready to use."
echo ""
