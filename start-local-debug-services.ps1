
Start-Process stripe -ArgumentList "listen", "--forward-to", "localhost:5000/Checkout/Webhook"

Set-Location .\ErabliereApi\

Start-Process dotnet -ArgumentList "watch", "run", "$PWD\ErabliereApi.csproj"

Set-Location ..
Set-Location ErabliereIU

Start-Process npm -ArgumentList "start"

Set-Location ..
