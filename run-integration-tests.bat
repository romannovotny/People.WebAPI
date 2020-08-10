docker-compose -f docker-compose-integrationtests.yml build &&^
docker-compose -f docker-compose-integrationtests.yml up --force-recreate --abort-on-container-exit