git status
git pull

Start-Process stripe -ArgumentList "listen", "--forward-to", "localhost:5000/Checkout/Webhook"

Start-Sleep 5

# check if stripe is running
$stripe = Get-Process | Where-Object { $_.Name -eq "stripe" }
if ($null -eq $stripe) {
    stripe login

    Start-Process stripe -ArgumentList "listen", "--forward-to", "localhost:5000/Checkout/Webhook"
}

Set-Location .\ErabliereApi\

Start-Process dotnet -ArgumentList "watch", "run", "$PWD\ErabliereApi.csproj", "ASPNETCORE_ENVIRONMENT=Development", "ASPNETCORE_HTTP_PORTS=5000", "ASPNETCORE_HTTPS_PORTS=5001", " --no-hot-reload"

Set-Location ..
Set-Location ErabliereIU

Start-Process npm -ArgumentList "start"

Start-Process npm -ArgumentList "run", "storybook"

Start-Process npx -ArgumentList "cypress", "open"

Set-Location ..

code .

# if the parent folder contains a folder name LearnNestJS, then start the NestJS server in a new process, also in watch mode

$learnNestJS = Get-ChildItem -Path ..\ -Directory -Filter "LearnNestJS"
if ($null -ne $learnNestJS) {
    Set-Location $learnNestJS
    Start-Process npm -ArgumentList "run", "start:dev"

    Set-Location ..\ErabliereApi
}

# if the parent folder contains a folder name EmailImagesObserver, then start the EmailImagesObserver server in a new process, also in watch mode

$emailImagesObserver = Get-ChildItem -Path ..\ -Directory -Filter "EmailImagesObserver"
if ($null -ne $emailImagesObserver) {
    Set-Location "$emailImagesObserver\BlazorApp"
    Start-Process dotnet -ArgumentList "watch", "run", "$PWD\BlazorApp.csproj", " --no-hot-reload"

    Set-Location ..\..\ErabliereApi
}