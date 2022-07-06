function Get-CrossPlatformIpAddress() {
    $os = [System.Environment]::OSVersion.Platform;
    if ($os -eq "Win32NT") {
        return (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias Ethernet).IPAddress;
    }
    else {
        $textLine = (ip address | grep eth0)[1]

        $regex = ‘\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b’
        
        $ipAddress = ($textLine | select-string -Pattern $regex -AllMatches | ForEach-Object { $_.Matches } | ForEach-Object { $_.Value })[0]

        return $ipAddress
    }
}