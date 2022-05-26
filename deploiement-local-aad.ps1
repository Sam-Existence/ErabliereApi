# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

$ErrorActionPreference = "Stop"

# Source: https://dev.to/onlyann/user-password-generation-in-powershell-core-1g91
function GeneratePassword {
    param(
        [ValidateRange(12, 256)]
        [int] 
        $length = 14
    )

    $symbols = '!@#$%^&*'.ToCharArray()
    $characterList = 'a'..'z' + 'A'..'Z' + '0'..'9' + $symbols

    do {
        $password = -join (0..$length | ForEach-Object { $characterList | Get-Random })
        [int]$hasLowerChar = $password -cmatch '[a-z]'
        [int]$hasUpperChar = $password -cmatch '[A-Z]'
        [int]$hasDigit = $password -match '[0-9]'
        [int]$hasSymbol = $password.IndexOfAny($symbols) -ne -1

    }
    until (($hasLowerChar + $hasUpperChar + $hasDigit + $hasSymbol) -ge 3)

    $password | ConvertTo-SecureString -AsPlainText
}

Write-Output "Generate a .env file"
Add-Type -AssemblyName System.Web
$envPassword = GeneratePassword
$envPath = $PWD.Path + "\" + ".env"
$ipAddress = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias Ethernet).IPAddress;
#$envContent = "SAPASSWORD=" + $envPassword + [System.Environment]::NewLine + "IP_ADDRESS=" + $ipAddress + [System.Environment]::NewLine;

# Create a string builder name $envContent and add password and ip address
$envContent = New-Object System.Text.StringBuilder
$envContent.Append("SAPASSWORD=" + $envPassword + [System.Environment]::NewLine)
$envContent.Append("IP_ADDRESS=" + $ipAddress + [System.Environment]::NewLine)

# Read the file at location C:\Config\ErabliereApi-Local\oidc-info.txt as a json object
$oidcConfig = Get-Content C:\Configs\ErabliereApi-Local\oidc-info.txt | ConvertFrom-Json

# Add the azure ad variable in the .env file using the string builder
$envContent.Append("TENANT_ID=" + $oidcConfig.TenantId + [System.Environment]::NewLine)
$envContent.Append("API_CLIENT_ID=" + $oidcConfig.ApiClientId + [System.Environment]::NewLine)
$envContent.Append("SWAGGER_CLIENT_ID=" + $oidcConfig.SwaggerClientId + [System.Environment]::NewLine)
$envContent.Append("SWAGGER_SCOPES=" + $oidcConfig.SwaggerScopes + [System.Environment]::NewLine)

# TODO: Push the IU ClientId into the oidc-config.json file


Write-Output $envContent
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllText($envPath, $envContent, $Utf8NoBomEncoding);

Write-Output "Generate config/oauth-oidc.json"
$oauthConfigOidcTemplatePath = $PWD.Path + "\config\" + "oauth-oidc.template.aad.json"
$oauthConfigOidcDestinationPath = $PWD.Path + "\config\" + "oauth-oidc.json"
$oauthConfigOidcTemplateContent = (Get-Content $oauthConfigOidcTemplatePath -Raw -Encoding utf8).Replace("<ip-address>", $ipAddress)
[System.IO.File]::WriteAllText($oauthConfigOidcDestinationPath, $oauthConfigOidcTemplateContent, $Utf8NoBomEncoding)