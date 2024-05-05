# ErabliereApi.IdentityServer

This is the IdentityServer project for the ErabliereApi solution. This project is intended to be used as the authentication and authorization server in a test environment.

## Docker

Build the docker image with the following command:

```powershell
docker build -t erabliereapi-identityserver .
```

Run the docker image with the following command:

```powershell
docker run -d -p 443:5005 --name erabliereapi-identityserver erabliereapi-identityserver
```