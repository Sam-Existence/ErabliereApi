# Parse the first args as the api key

param(
    [Parameter(Mandatory=$true)]
    [string]$apiKey
)

$headers = @{ "X-Key"= $apiKey }

# Run the ncu command and store the output in a variable
$ncuOutput = ncu --format lines

# Replace the tabs to use the windows endline
$ncuOutput = $ncuOutput -replace "  ", [System.Environment].NewLine

Write-Host "ncuOutput:" $ncuOutput

# Filter out the typescript package from the output
$filteredOutput = $ncuOutput | Where-Object { $_ -notlike "*typescript*" }

Write-Host "filteredOutput:" $filteredOutput

Invoke-WebRequest -Uri "https://api.newreleases.io/v1/projects" -Headers $headers

# Fetch the release notes for the packages that can be updated
foreach ($line in $filteredOutput) {
    Write-Host "line:" $line

    # split the line by the @ character
    $name = ($line -split "@")[0]

    # use an api to get the release notes
    $url = "https://api.newreleases.io/v1/project/$name/$name"

    # print the url
    Write-Host $url

    # fetch the release notes using the url and apiKey
    $releaseNotes = Invoke-WebRequest -Uri $url -Headers $headers | ConvertFrom-Json

    # print release notes
    Write-Host $releaseNotes.collected.metadata.releaseNotes
}
