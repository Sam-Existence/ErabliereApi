Set-Location .\ErabliereApi\

Start-Process dotnet -ArgumentList "watch", "run", "$PWD\ErabliereApi.csproj", "ASPNETCORE_ENVIRONMENT=Development", "ASPNETCORE_HTTP_PORTS=5000", "ASPNETCORE_HTTPS_PORTS=5001", " --no-hot-reload"

Set-Location ..
Set-Location ErabliereIU

Start-Process npm -ArgumentList "start"

Start-Process "C:\Program Files\JetBrains\WebStorm 2023.3.4\bin\webstorm64.exe" .