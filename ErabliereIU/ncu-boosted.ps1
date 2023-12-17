# Run the ncu command and store the output in a variable
$ncuOutput = ncu --format lines

# Replace the tabs to use the windows endline
$ncuOutput = $ncuOutput -replace "  ", [System.Environment].NewLine

Write-Host "ncuOutput:" $ncuOutput

# Filter out the typescript package from the output
$filteredOutput = $ncuOutput | Where-Object { $_ -notlike "*typescript*" }

Write-Host "filteredOutput:" $filteredOutput

# Fetch the release notes for the packages that can be updated
$releaseNotes = $filteredOutput | ForEach-Object {
    Write-Host "Fetching release notes for $_"
    $packageName = ($_ -split " ")[0]
    
    # Fetch the release notes for the package using the github api
    $releaseNote = Invoke-RestMethod -Uri "https://api.github.com/repos/$packageName/releases/latest" -Method Get

    # Return the package name and the release notes
    return "$packageName, " + $releaseNote.collected.metadata.release
}

# Print the release notes
$releaseNotes
