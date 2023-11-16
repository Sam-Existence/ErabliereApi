
Write-Hot 'Step 1: Find all Dockerfiles and scprods that contain ".net7.0|/7.0".'
$filesToUpdate = Get-ChildItem -Recurse -Include Dockerfile, *.csprod | Select-String -Pattern ".net7.0|/7.0"

Write-Host 'Step 2: Replace ".net7.0|/7.0" with ".NET8.0" in the found files.'
foreach ($file in $filesToUpdate) {
    (Get-Content $file.Path) | Foreach-Object {
        $_ -replace ".net7.0|/7.0"
    } | Set-Content $file.Path
}

Write-Host 'Step 3: Test the build.'
dotnet build

# Step 5: If there are any errors, fix them automatically.
if ($LASTEXITCODE -ne 0) {
    dotnet restore
    dotnet build
}
