#!/bin/bash
echo "Starting service update"
cd Scripts/ || return
docker-compose pull
docker-compose up -d