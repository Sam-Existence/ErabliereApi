# ErabliereAPI.Proxy

Client proxy pour l'api ErabliereAPI.

## Generate new version

1. Get nswag studioL ```choco install nswagstudio```
2. Load the nswag file
3. Generate the client

## How to use

```csharp
var client = new ErabliereAPIProxy("https://localhost:5001", new HttpClient());
var result = await client.GetWeatherAsync();
```