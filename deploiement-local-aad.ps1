# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

$ErrorActionPreference = "Stop"

Write-Output "Generate a .env file"
Add-Type -AssemblyName System.Web
$envPassword = GeneratePassword
$envPath = $PWD.Path + "\" + ".env"
$ipAddress = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias Ethernet).IPAddress;
$envContent = "SAPASSWORD=" + $envPassword + [System.Environment]::NewLine + "IP_ADDRESS=" + $ipAddress;
Write-Output $envContent
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllText($envPath, $envContent, $Utf8NoBomEncoding);

Write-Output "Generate config/oauth-oidc.json"
$oauthConfigOidcTemplatePath = $PWD.Path + "\config\" + "oauth-oidc.aad.template.json"
$oauthConfigOidcDestinationPath = $PWD.Path + "\config\" + "oauth-oidc.json"
$oauthConfigOidcTemplateContent = (Get-Content $oauthConfigOidcTemplatePath -Raw -Encoding utf8).Replace("<ip-address>", $ipAddress)
[System.IO.File]::WriteAllText($oauthConfigOidcDestinationPath, $oauthConfigOidcTemplateContent, $Utf8NoBomEncoding)