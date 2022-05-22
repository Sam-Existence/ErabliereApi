# Build the angular app
FROM node:16.15.0-alpine AS angular-builder
WORKDIR /usr/src/app
COPY ErabliereIU/package.json ErabliereIU/package-lock.json ./
RUN npm install --legacy-peer-deps
RUN npm install -g @angular/cli
COPY ErabliereIU/ .
RUN ng build --configuration production

# Build the api
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-api-env
WORKDIR /app

COPY ErabliereModel/*.csproj ./ErabliereModel/
COPY ErabliereApi/*.csproj ./ErabliereApi/
COPY ErabliereApi.Test/*.csproj ./ErabliereApi.Test/
COPY ErabliereApi.Integration.Test/*.csproj ./ErabliereApi.Integration.Test/

COPY ErabliereModel/. ./ErabliereModel/
COPY ErabliereApi/. ./ErabliereApi/
COPY ErabliereApi.Test/. ./ErabliereApi.Test/
COPY ErabliereApi.Integration.Test/. ./ErabliereApi.Integration.Test/

WORKDIR /app/ErabliereApi
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet test ../ErabliereApi.Test/ErabliereApi.Test.csproj -c Release
RUN dotnet test ../ErabliereApi.Integration.Test/ErabliereApi.Integration.Test.csproj -c Release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-api-env /app/ErabliereApi/out ./
RUN chmod u+x docker-entrypoint.sh
COPY --from=angular-builder /usr/src/app/dist/ErabliereIU ./wwwroot
RUN rm ./wwwroot/assets/config/oauth-oidc.json && mv ./wwwroot/assets/config/oauth-oidc-docker.json ./wwwroot/assets/config/oauth-oidc.json

# Expose port
EXPOSE 443
EXPOSE 80

#Create a new user (erabliereapp) and new group (erabliereapi); then switch into that user’s context 
#RUN useradd erabliereapp && groupadd erabliereapi 
#USER erabliereapp:erabliereapi 

ENTRYPOINT ["dotnet", "ErabliereApi.dll"]
