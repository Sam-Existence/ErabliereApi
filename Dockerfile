# Build the angular app
FROM node:12.16.1-alpine As andular-builder
WORKDIR /usr/src/app
COPY ErabliereIU/package.json ErabliereIU/package-lock.json ./
RUN npm install
RUN npm install -g @angular/cli
COPY ErabliereIU/ .
RUN ng build --prod

# Build the api
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.sln  .
COPY ErabliereModel/*.csproj ./ErabliereModel/
COPY ErabliereApi/*.csproj ./ErabliereApi/

RUN dotnet restore

COPY ErabliereModel/. ./ErabliereModel/
COPY ErabliereApi/. ./ErabliereApi/

WORKDIR /app/ErabliereApi
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/ErabliereApi/out ./
COPY --from=andular-builder /usr/src/app/dist/ErabliereIU ./wwwroot
ENTRYPOINT ["dotnet", "ErabliereApi.dll"]