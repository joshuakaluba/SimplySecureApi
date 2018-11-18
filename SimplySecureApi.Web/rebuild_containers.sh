#!/bin/bash
clear
docker-compose stop
docker-compose build
docker-compose up -d
echo "rebuilding containers done!"
