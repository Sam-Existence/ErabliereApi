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

$yamlContentPath = $PWD.Path + "\cronjob-getinfo-hmi.yaml"
$yamlContent = Get-Content -Path $yamlContentPath -Encoding UTF8 -Raw

$yamlContent = Replace-UserVariable "Entrer l'addresse du hmi:" "<hmi-web-address>" $yamlContent
$yamlContent = Replace-UserVariable "Entrer l'addresse de l'api à l'action POST:" "<rest-api-post-address>" $yamlContent
$yamlContent = Replace-UserVariable "Entrer l'id de l'érablière:" "<erabliere-guid-id>" $yamlContent
$namespace = Get-UserVariable "Entrer le namespace de la cronjob:"

$yamlContent | kubectl apply -n $namespace -f -