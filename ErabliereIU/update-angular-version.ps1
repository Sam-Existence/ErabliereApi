Write-Host "A script to update the angular version in the project"

$version = "latest"

Write-Host "ng update preview"
ng update

Write-Host "Update the angular version $version"
ng update @angular/cli@$version --force --allow-dirty
ng update @angular/core@$version --force --allow-dirty
ng update @angular-eslint/schematics@$version --force --allow-dirty

npm install typescript@latest