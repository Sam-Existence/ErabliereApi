[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]$newmanEnvironement=".\Postman\ErabliereAPI-Local.postman_environment.json",
    [Parameter(Mandatory=$false)]
    [string]$frontEndUrl="https://192.168.0.110:5001",
    [Parameter(Mandatory=$false)]
    [bool]$skipInstall=$false
)

$initialLocation = $PWD.Path

Write-Host "Initial location: $initialLocation"

Remove-Item .\TestResults\*.xml

if (-not($skipInstall)) {
    npm install -g newman
}

newman run .\Postman\ErabliereAPI.postman_collection.json -e $newmanEnvironement --reporters 'cli,junit' --reporter-junit-export .\TestResults\newman-junit-report.xml --insecure

Set-Location ErabliereIU

if (-not($skipInstall)) {
    npm install
}

npx cypress run --config baseUrl=$frontEndUrl --reporter junit --reporter-options "mochaFile=..\TestResults\cypress-test-output-[hash].xml"

Set-Location $initialLocation

# Check if there is some errors in the test results
# Loop through the test results and check if there is any error
$exitCode = 0
foreach ($result in (Get-ChildItem -Path "$initialLocation\TestResults" -Filter "*.xml")) {
    $xml = [xml](Get-Content ("$initialLocation\TestResults\" + [System.IO.Path]::GetFileName($result)))

    Write-Host $result
    if ($xml.testsuites.failures -gt 0) {
        Write-Host "There are some errors in the test results"
        Write-Host $xml.testsuites.failures "/" $xml.testsuites.tests "failures"
        $exitCode = 1
    }
    else {
        Write-Host "All" $xml.testsuites.tests "tests passed !" 
    }
}

exit $exitCode