Write-Host "Running npx storybook@latest upgrade"
npx storybook@latest upgrade

Write-Host "Running npm dedupe"
npm dedupe

Write-Host "Update chromatic"
npm install chromatic@latest