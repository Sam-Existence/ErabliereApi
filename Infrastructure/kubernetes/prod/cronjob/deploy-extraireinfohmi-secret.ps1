function Get-UserVariable([string] $message) {
    Write-Host $message
    $variable = Read-Host
    return $variable
}

function Replace-UserVariable([string] $message, [string] $variableName, [string] $source) {
    Write-Host $message
    $variable = Read-Host
    $string = $source.Replace($variableName, $variable)
    return $string
}

$yamlContentPath = $PWD.Path + "\aad-json-template.json"
$yamlContent = Get-Content -Path $yamlContentPath -Encoding UTF8 -Raw

# Gestion des clients credentials de AzureAD
$yamlContent = Replace-UserVariable "Entrer le tenantId:" "<tenantId>" $yamlContent
$yamlContent = Replace-UserVariable "Entrer le clientId:" "<clientId>" $yamlContent
$yamlContent = Replace-UserVariable "Entrer le clientSecret:" "<clientSecret>" $yamlContent
$yamlContent = Replace-UserVariable "Entrer le apiClientId:" "<api-clientId>" $yamlContent
$namespace = Get-UserVariable "Entrer le namespace du secret:"

$yamlContent2Path = $PWD.Path + "\cronjob-authentification-secret.yaml"
$yamlContent2 = Get-Content -Path $yamlContent2Path -Encoding UTF8 -Raw

# convertire le yamlContent en base64
$yamlContent = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($yamlContent))

# remplacer le contenu du yamlContent dans le yamlContent2
$yamlContent2 = $yamlContent2.Replace("<auth-config-base64>", $yamlContent)

# gestion de la clé d'api acuweather
$apiKey = Get-UserVariable "Entrer le apiKey acuweather:"
# convertire la clé en base64
$apiKey = [Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes($apiKey))
# remplacer dans le yaml
$yamlContent2 = $yamlContent2.Replace("<acuweather-apikey-base64>", $apiKey)

$yamlContent2 | kubectl apply -n $namespace -f -