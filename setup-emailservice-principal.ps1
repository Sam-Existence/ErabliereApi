param(
    [string] $tenantId = "c383c74f-8edf-4f51-9bf3-c2bc352996bd",
    [string] $appId = "4f782545-4370-417d-a0f8-eba950872a13",
    [string] $objectId = "26c24956-2657-467d-8e58-d9cc131d6726",
    [string] $identity = "frederic.jacques@freddycoder.com",
    [string] $appName = "ErabliereAPI-Debug-MultiTenant"
)

Install-Module -Name ExchangeOnlineManagement
Import-Module ExchangeOnlineManagement
Connect-ExchangeOnline -Organization $tenantId

New-ServicePrincipal -AppId $appId -ObjectId $objectId

Get-ServicePrincipal | Format-List

Add-MailboxPermission -Identity $identity -User $servicePrincipalId -AccessRights FullAccess

$AADServicePrincipalDetails = Get-AzureADServicePrincipal -SearchString $appName

New-ServicePrincipal -AppId $AADServicePrincipalDetails.AppId -ObjectId $AADServicePrincipalDetails.ObjectId -DisplayName "EXO Serviceprincipal for AzureAD App $($AADServicePrincipalDetails.Displayname)"

$EXOServicePrincipal = Get-ServicePrincipal -Identity "EXO Serviceprincipal for AzureAD App $($AADServicePrincipalDetails.Displayname)"

Add-MailboxPermission -Identity $identity -User $EXOServicePrincipal.Identity -AccessRights FullAccess