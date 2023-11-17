# Build the api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ErabliereApi.IdentityServer.csproj ./

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out ./

# Expose port
EXPOSE 443

# Setup the entrypoint
ENTRYPOINT ["dotnet", "ErabliereApi.IdentityServer.dll"]