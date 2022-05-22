param(
    [string]$newmanEnvironement=".\Postman\ErabliereAPI-Local.postman_environment.json",
    [string]$frontEndUrl="https://192.168.0.110:5001"
)

npm install -g newman

newman run .\Postman\ErabliereAPI.postman_collection.json -e $newmanEnvironement --insecure

$initialLocation = $PWD.Path

Set-Location ErabliereIU

npm install --legacy-peer-deps

npx cypress run --config baseUrl=$frontEndUrl

Set-Location $initialLocation