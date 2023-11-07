git status
git pull

Start-Process stripe -ArgumentList "listen", "--forward-to", "localhost:5000/Checkout/Webhook"

# Start-Process dotnet -ArgumentList "run", "--project", "$PWD\IdentityServer\ErabliereApi.IdentityServer\ErabliereApi.IdentityServer.csproj"

Set-Location .\ErabliereApi\

Start-Process dotnet -ArgumentList "watch", "run", "$PWD\ErabliereApi.csproj"

Set-Location ..
Set-Location ErabliereIU

npm install

Start-Process npm -ArgumentList "start"

Start-Process npm -ArgumentList "run", "storybook"

Start-Process npx -ArgumentList "cypress", "open"

Set-Location ..

code .

