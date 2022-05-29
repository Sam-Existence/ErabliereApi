.\deploiement-local.ps1 -skipCertificateCreation $true
docker compose pull
docker compose up -d --force-recreate