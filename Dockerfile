FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY ErabliereApi.sln  ./
COPY ErabliereModel/ErabliereApi.Donnees.csproj ./ErabliereModel
COPY ErabliereApi/ErabliereApi.csproj ./ErabliereApi
RUN dotnet restore

COPY ErabliereModel/* ./ErabliereModel
COPY ErabliereApi/* ./ErabliereApi

WORKDIR /ErabliereModel
RUN dotnet publish -c Release -o /app

WORKDIR /ErabliereApi
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "ErabliereApi.dll"]