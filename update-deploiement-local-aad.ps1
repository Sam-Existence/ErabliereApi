param(
    [string] $dockerComposeFile = "docker-compose-aad.yaml"
)

docker compose -f $dockerComposeFile pull 
docker compose -f $dockerComposeFile up -d --force-recreate