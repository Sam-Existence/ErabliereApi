. ..\local\common\ip-util.ps1

Remove-Item *.txt

$ip = Get-CrossPlatformIpAddress

$network = Remove-LastIpOctet $ip

for ($i = 1; $i -le 254; $i++) {
    # ping the IP address
    $ping = New-Object System.Net.NetworkInformation.Ping
    $pingReply = $ping.Send("$network.$i", 30)

    Write-Host "Pinging $network.$i"

    if ($pingReply.Status -eq "Success") {
        $pingReply | Out-File -FilePath "$network.$i.txt"
    }
}

# check if nmap is installed
$nmapInstalled = Get-Command nmap -ErrorAction SilentlyContinue
if ($nmapInstalled) {
   # Foreach file in the current directory with name match ip address .txt
    Get-ChildItem -Path . -Filter *.txt | ForEach-Object {
        # Read the file
        $targetIp = $_.BaseName.Replace(".txt", "")
        # Do a nmap os scan on the target IP
        Write-Host "Nmap OS scan on $targetIp", "start at", (Get-Date)
        nmap -A $targetIp | Out-File -FilePath "$targetIp.txt" -Append
    }
}
else {
    Write-Host "Nmap is not installed"
}


arp -a > arp.txt