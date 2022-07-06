# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

$ErrorActionPreference = "Stop"

. .\Infrastructure\local\common\generate-password.ps1
. .\Infrastructure\local\common\ip-util.ps1
. .\Infrastructure\local\common\os-util.ps1

Write-Output "Generate a .env file"
Add-Type -AssemblyName System.Web
$envPassword = (New-Password)[0]
$envPath = ConvertTo-CrossPlatformPath($PWD.Path + "\" + ".env")
$ipAddress = Get-CrossPlatformIpAddress

# Create a string builder name $envContent and add password and ip address
$envContent = New-Object System.Text.StringBuilder
$envContent.Append("SAPASSWORD=" + $envPassword + [System.Environment]::NewLine)
$envContent.Append("IP_ADDRESS=" + $ipAddress + [System.Environment]::NewLine)

# Read the file at location C:\Config\ErabliereApi-Local\oidc-info.txt as a json object
$oidcConfig = {}

if ($true -eq (Get-IsWindowsOS)) {
    $oidcConfig = Get-Content C:\Configs\ErabliereApi-Local\oidc-info.txt | ConvertFrom-Json
}
else {
    $oidcConfig = Get-Content /mnt/c/Configs/ErabliereApi-Local/oidc-info.txt | ConvertFrom-Json
}

# Add the azure ad variable in the .env file using the string builder
$envContent.Append("TENANT_ID=" + $oidcConfig.TenantId + [System.Environment]::NewLine)
$envContent.Append("API_CLIENT_ID=" + $oidcConfig.ApiClientId + [System.Environment]::NewLine)
$envContent.Append("SWAGGER_CLIENT_ID=" + $oidcConfig.SwaggerClientId + [System.Environment]::NewLine)
$envContent.Append("SWAGGER_SCOPES=" + $oidcConfig.Scopes + [System.Environment]::NewLine)

Write-Output $envContent
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllText($envPath, $envContent, $Utf8NoBomEncoding);

Write-Output "Generate config/oauth-oidc.json"
$oauthConfigOidcTemplatePath = ConvertTo-CrossPlatformPath($PWD.Path + "\config\" + "oauth-oidc.template.aad.json")
$oauthConfigOidcDestinationPath = ConvertTo-CrossPlatformPath($PWD.Path + "\config\" + "oauth-oidc.json")
$oauthConfigOidcTemplateContent = (Get-Content $oauthConfigOidcTemplatePath -Raw -Encoding utf8).Replace("<ip-address>", $ipAddress).Replace("<client-id>", $oidcConfig.IUClientId).Replace("<tenant-id>", $oidcConfig.TenantId).Replace("<iu-scopes>", $oidcConfig.Scopes)
[System.IO.File]::WriteAllText($oauthConfigOidcDestinationPath, $oauthConfigOidcTemplateContent, $Utf8NoBomEncoding)