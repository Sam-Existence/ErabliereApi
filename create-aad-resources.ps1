Write-Output "Make sure you run the script in a powershell shell and not a powershell core shell"
Write-Output "Also make sure you run powershell as administrator"

# Install and import module
Find-Module AzureAD
Install-Module -Name AzureAD
Import-Module AzureAD

# Connect to AzureAD
$AzureADCredentials = Get-Credential -Message "Credential to connect to Azure AD"
Connect-AzureAD -Credential $AzureADCredentials

# Get basic information
Get-AzureADCurrentSessionInfo
Get-AzureADTenantDetail
Get-AzureADDomain

# To list all command
#Get-Command -Module AzureAD
$ipAddress = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias Ethernet).IPAddress;

$app = New-AzureADApplication -DisplayName erabliereapi-local -IdentifierUri https://${ipAddress}:5001/

