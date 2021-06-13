# Build the angular app
FROM node:12.16.1-alpine AS angular-builder
WORKDIR /usr/src/app
COPY ErabliereIU/package.json ErabliereIU/package-lock.json ./
RUN npm install
RUN npm install -g @angular/cli
COPY ErabliereIU/ .
RUN ng build --prod

# Build the api
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-api-env
WORKDIR /app

COPY ErabliereModel/*.csproj ./ErabliereModel/
COPY ErabliereApi/*.csproj ./ErabliereApi/
COPY ErabliereApi.Test/*.csproj ./ErabliereApi.Test/

# RUN dotnet restore ErabliereModel/*.csproj
# RUN dotnet restore ErabliereApi/*.csproj
# RUN dotnet restore ErabliereApi.Test/*.csproj

COPY ErabliereModel/. ./ErabliereModel/
COPY ErabliereApi/. ./ErabliereApi/
COPY ErabliereApi.Test/. ./ErabliereApi.Test/

WORKDIR /app/ErabliereApi
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-api-env /app/ErabliereApi/out ./
COPY --from=angular-builder /usr/src/app/dist/ErabliereIU ./wwwroot

# Expose port
EXPOSE 443
EXPOSE 80

ENTRYPOINT ["dotnet", "ErabliereApi.dll"]
