param(
    [string] $dockerComposeFile = "docker-compose-aad.yaml"
)

.\Infrastructure\local\deploiement-local-aad.ps1
docker compose -f $dockerComposeFile pull 
docker compose -f $dockerComposeFile up -d --force-recreate --remove-orphans