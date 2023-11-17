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

# Breaking change dotnet 8
# https://learn.microsoft.com/en-us/aspnet/core/migration/70-80?view=aspnetcore-8.0&tabs=visual-studio#update-docker-portss
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_HTTPS_PORTS=443

RUN dotnet dev-certs https --trust

# Expose port
EXPOSE 443

# Setup the entrypoint
ENTRYPOINT ["dotnet", "ErabliereApi.IdentityServer.dll"]