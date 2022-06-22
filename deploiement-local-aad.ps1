# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

$ErrorActionPreference = "Stop"

function New-Password {
    param(
        [ValidateRange(12, 256)]
        [int] $length = 14
    )

    $specilChar = '!"/$%?&*()_-'

    $builder = New-Object -TypeName System.Text.StringBuilder

    while ($builder.Length -lt $length) {
        $builder.Append([System.Guid]::NewGuid().ToString().Replace('-', ''))
    }

    if ($builer.Length -gt $length) {
        $builder.Remove($length, $builder.Length - $length);
    }
    
    $changeRandomNumberOfChar = Get-Random -Maximum ($length - 1) -Minimum ($length / 3)

    for ($i = 0; $i -lt $changeRandomNumberOfChar; $i++) {
        $toUpperOrSpecialChar = Get-Random -Minimum 1 -Maximum 3

        $index = Get-Random -Minimum 0 -Maximum $length

        if ($toUpperOrSpecialChar -ge 2) {
            $builder[$index] = $builder[$index].ToString().ToUpper()[0]
        }
        else {
            $randomSpecialCharIndex = Get-Random -Minimum 0 -Maximum $specilChar.Length
            $builder[$index] = $specilChar[$randomSpecialCharIndex]
        }
    }

    return $builder
}

Write-Output "Generate a .env file"
Add-Type -AssemblyName System.Web
$envPassword = (New-Password)[0]
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
$envContent.Append("SWAGGER_SCOPES=" + $oidcConfig.Scopes + [System.Environment]::NewLine)

Write-Output $envContent
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllText($envPath, $envContent, $Utf8NoBomEncoding);

Write-Output "Generate config/oauth-oidc.json"
$oauthConfigOidcTemplatePath = $PWD.Path + "\config\" + "oauth-oidc.template.aad.json"
$oauthConfigOidcDestinationPath = $PWD.Path + "\config\" + "oauth-oidc.json"
$oauthConfigOidcTemplateContent = (Get-Content $oauthConfigOidcTemplatePath -Raw -Encoding utf8).Replace("<ip-address>", $ipAddress).Replace("<client-id>", $oidcConfig.IUClientId).Replace("<tenant-id>", $oidcConfig.TenantId).Replace("<iu-scopes>", $oidcConfig.Scopes)
[System.IO.File]::WriteAllText($oauthConfigOidcDestinationPath, $oauthConfigOidcTemplateContent, $Utf8NoBomEncoding)