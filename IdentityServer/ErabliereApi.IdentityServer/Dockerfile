# Build the api
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY ErabliereApi.IdentityServer.csproj ./

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

# Breaking change dotnet 8
# https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-8.0#macos-or-linux
RUN dotnet dev-certs https -ep /app/.aspnet/https/aspnetapp.pfx -p password
RUN dotnet dev-certs https --trust

# Breaking change dotnet 8
# https://learn.microsoft.com/en-us/aspnet/core/migration/70-80?view=aspnetcore-8.0&tabs=visual-studio#update-docker-portss
ENV ASPNETCORE_HTTP_PORTS=80
ENV ASPNETCORE_HTTPS_PORTS=443

# Expose port
EXPOSE 443

WORKDIR /app/out

# Setup the entrypoint
ENTRYPOINT ["dotnet", "ErabliereApi.IdentityServer.dll"]