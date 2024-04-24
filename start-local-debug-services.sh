#!/bin/bash

git status
git pull

# Start stripe
stripe listen --forward-to localhost:5000/Checkout/Webhook &

# Wait for stripe to start (5 seconds)
sleep 5

# Check if stripe is running
if ! pgrep -x "stripe" > /dev/null
then
    stripe login
    stripe listen --forward-to localhost:5000/Checkout/Webhook &
fi

cd ErabliereApi

dotnet watch run ErabliereApi.csproj -- --ASPNETCORE_ENVIRONMENT=Development --ASPNETCORE_HTTP_PORTS=5000 --ASPNETCORE_HTTPS_PORTS=5001 --no-hot-reload &

cd ..
cd ErabliereIU

npm start &
npm run storybook &

npx cypress open &

cd ..

code .