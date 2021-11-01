#!/bin/bash

echo "Setting up..."
PWD=`pwd`
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

cd $SCRIPT_DIR/..

export IPADDR="`ifconfig | grep "inet " | grep -Fv 127.0.0.1 | awk '{print $2}' | head -n 1`"

echo "Stopping test environment..."
docker compose --file docker-compose.test.yml stop

echo "Cleaning up environment."
rm emulatorcert.crt
cd $PWD

echo "Your test environment stopped."
echo ""
