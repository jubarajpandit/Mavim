#!/bin/bash

# install all packages
npm i

# run tests, generate report and get the exit code
./node_modules/.bin/cross-env USER_NAME=$1 USER_PASS=$2 HUB_HOST=$3 BASE_URL=$4 INSTANCES=$5 PRODUCTION="true" npm run test:prod || EXIT_CODE=$?

echo "WebDriverIO execute successfully."

sleep infinity