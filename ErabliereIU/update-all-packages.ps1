Write-Host "Previewing updates..."
npm i -g npm-check-updates
ncu

Write-Host "Updating angular"
. .\update-angular-version.ps1

npm install ngx-mask@latest
npm install @babel/core@latest

Write-Host "Update storybook"
. .\update-storybook.ps1
npm dedupe

npm install chromatic@latest

Write-Host "Finalize updates"
npm install @types/node@latest
npm install @typescript-eslint/eslint-plugin@latest
npm install @typescript-eslint/parser@latest

Write-Host "NCU again"
ncu