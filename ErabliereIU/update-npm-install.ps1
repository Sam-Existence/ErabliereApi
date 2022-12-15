Write-Output "Remove node_modules folder"
Remove-Item -Path "node_modules" -Recurse -Force
Write-Output "Remove package-lock.json"
Remove-Item -Path "package-lock.json" -Force
Write-Output "npm install"
npm install