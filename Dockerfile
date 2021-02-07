FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY *.sln  .
COPY ErabliereModel/*.csproj ./ErabliereModel
COPY GenerateurDeDonnées/*.csproj ./GenerateurDeDonnées
COPY ErabliereApi/*.csproj ./ErabliereApi

RUN dotnet restore

COPY ErabliereModel/. ./ErabliereModel/
COPY GenerateurDeDonnées/ ./GenerateurDeDonnées/
COPY ErabliereApi/. ./ErabliereApi/

WORKDIR /app/ErabliereApi
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/ErabliereApi/out ./
ENTRYPOINT ["dotnet", "ErabliereApi.dll"]