Write-Host "Previewing updates..."
npm i -g npm-check-updates
ncu

Write-Host "Updating angular"
. .\update-angular-version.ps1

Write-Host "Update storybook"
. .\update-storybook.ps1
npm dedupe

ncu -u

npm install