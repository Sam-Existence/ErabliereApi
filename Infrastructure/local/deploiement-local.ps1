# Generate certificat for https
# Source: https://github.com/mjarosie/IdentityServerDockerHttpsDemo/blob/master/generate_self_signed_cert.ps1
# Source: https://stackoverflow.com/a/62060315
# Generate self-signed certificate to be used by IdentityServer.
# When using localhost - API cannot see the IdentityServer from within the docker-compose'd network.
# You have to run this script as Administrator (open Powershell by right click -> Run as Administrator).

param(
    [bool] $skipCertificateCreation = $false
)

$ErrorActionPreference = "Stop"

. .\Infrastructure\local\common\generate-password.ps1
. .\Infrastructure\local\common\cert-util.ps1
. .\Infrastructure\local\common\ip-util.ps1

Write-Output "******************************************************"
Write-Output "ErabliereAPI docker desktop setup"
Write-Output "Using IdentityServer as authentication provider"
Write-Output "******************************************************"
Write-Output ""
Write-Output "******************************************************"
Write-Output "make sure to run the script as administrator"
Write-Output "using powershell core"
Write-Output "******************************************************"

if (-not($skipCertificateCreation)) {
    $rootCN = "ErabliereAPIDockerSSLSetup"
    $identityServerCNs = "identite-api", "localhost"
    $webApiCNs = "erabliere-api", "localhost"

    $alreadyExistingCertsRoot = Get-MyCertificat $rootCN
    $alreadyExistingCertsIdentityServer = Get-MyCertificat $identityServerCNs[0]
    $alreadyExistingCertsApi = Get-MyCertificat $webApiCNs[0]

    if ($alreadyExistingCertsRoot.Count -eq 1) {
        Write-Output "Skipping creating Root CA certificate as it already exists."
        $testRootCA = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsRoot[0]
    } else {
        $testRootCA = New-SelfSignedCertificate -Subject $rootCN -KeyUsageProperty Sign -KeyUsage CertSign -CertStoreLocation Cert:\LocalMachine\My
    }

    if ($alreadyExistingCertsIdentityServer.Count -eq 1) {
        Write-Output "Skipping creating Identity Server certificate as it already exists."
        $identityServerCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsIdentityServer[0]
    } else {
        # Create a SAN cert for both identity-server and localhost.
        $identityServerCert = New-SelfSignedCertificate -DnsName $identityServerCNs -Signer $testRootCA -CertStoreLocation Cert:\LocalMachine\My
    }

    if ($alreadyExistingCertsApi.Count -eq 1) {
        Write-Output "Skipping creating API certificate as it already exists."
        $webApiCert = [Microsoft.CertificateServices.Commands.Certificate] $alreadyExistingCertsApi[0]
    } else {
        # Create a SAN cert for both web-api and localhost.
        $webApiCert = New-SelfSignedCertificate -DnsName $webApiCNs -Signer $testRootCA -CertStoreLocation Cert:\LocalMachine\My
    }

    # Export it for docker container to pick up later.
    $password = ConvertTo-SecureString -String "password" -Force -AsPlainText

    $rootCertPathPfx = $PWD.Path + "\" + "certs"
    $identityServerCertPath = $PWD.Path + "\" + "IdentityServer\ErabliereApi.IdentityServer\certs"
    $webApiCertPath = $PWD.Path + "\" + "ErabliereApi\certs"

    [System.IO.Directory]::CreateDirectory($rootCertPathPfx) | Out-Null
    [System.IO.Directory]::CreateDirectory($identityServerCertPath) | Out-Null
    [System.IO.Directory]::CreateDirectory($webApiCertPath) | Out-Null

    Export-PfxCertificate -Cert $testRootCA -FilePath "$rootCertPathPfx\aspnetapp-root-cert.pfx" -Password $password | Out-Null
    Export-PfxCertificate -Cert $identityServerCert -FilePath "$identityServerCertPath\aspnetapp-identity-server.pfx" -Password $password | Out-Null
    Export-PfxCertificate -Cert $webApiCert -FilePath "$webApiCertPath\aspnetapp-web-api.pfx" -Password $password | Out-Null

    # Export .cer to be converted to .crt to be trusted within the Docker container.
    $rootCertPathCer = "certs\aspnetapp-root-cert.cer"
    Export-Certificate -Cert $testRootCA -FilePath $rootCertPathCer -Type CERT | Out-Null

    # Trust it on your host machine.
    $store = New-Object System.Security.Cryptography.X509Certificates.X509Store "Root","LocalMachine"
    $store.Open("ReadWrite")

    $rootCertAlreadyTrusted = ($store.Certificates | Where-Object {$_.Subject -eq "CN=$rootCN"} | Measure-Object).Count -eq 1

    if ($rootCertAlreadyTrusted -eq $false) {
        $message = "Adding the root CA certificate to the trust store. (" + $rootCertPathPfx + "\aspnetapp-root-cert.pfx)"
        Write-Output $message
        $certBytes = [System.IO.File]::ReadAllBytes($rootCertPathPfx + "\aspnetapp-root-cert.pfx");
        $testRootCACert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($certBytes, $password)
        $store.Add($testRootCACert)
    }

    $store.Close()
}

Write-Output "Generate a .env file"
Add-Type -AssemblyName System.Web
# Generate a new password for the database, then check if if already exists in the .env file
# If that is the case, we keep the old password
$envPassword = (New-Password)[0]

$envFile = ".env"
$envFileContent = @{}
if (Test-Path $envFile) {
    $envFileContent = Get-Content $envFile | ConvertFrom-StringData
    $envPassword = $envFileContent.SAPASSWORD
}
$envPath = $PWD.Path + "\" + ".env"
$ipAddress = Get-CrossPlatformIpAddress
$envContent = "SAPASSWORD=" + $envPassword + [System.Environment]::NewLine + "IP_ADDRESS=" + $ipAddress;
Write-Output $envContent
$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
[System.IO.File]::WriteAllText($envPath, $envContent, $Utf8NoBomEncoding);

Write-Output "Generate config/oauth-oidc.json"
$oauthConfigOidcTemplatePath = $PWD.Path + "\config\" + "oauth-oidc.template.json"
$oauthConfigOidcDestinationPath = $PWD.Path + "\config\" + "oauth-oidc.json"
$oauthConfigOidcTemplateContent = (Get-Content $oauthConfigOidcTemplatePath -Raw -Encoding utf8).Replace("<ip-address>", $ipAddress)
[System.IO.File]::WriteAllText($oauthConfigOidcDestinationPath, $oauthConfigOidcTemplateContent, $Utf8NoBomEncoding);

docker compose up -d