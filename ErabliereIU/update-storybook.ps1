Write-Host "Running npx sb@next upgrade"
npx storybook upgrade

Write-Host "Running npm dedupe"
npm dedupe

Write-Host "Update chromatic"
npm install chromatic@latest