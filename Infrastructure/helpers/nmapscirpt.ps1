param(
    [Parameter(Mandatory=$true)]
    [string]$IpAddress
)

# Basic scan
Write-Output "Performing basic scan on $IpAddress..."
nmap $IpAddress

# OS detection
Write-Output "Performing OS detection on $IpAddress..."
nmap -O $IpAddress

# Service version detection
Write-Output "Performing service version detection on $IpAddress..."
nmap -sV $IpAddress

# Port scan
Write-Output "Performing port scan on $IpAddress..."
nmap -p- $IpAddress

# Script scan
Write-Output "Performing script scan on $IpAddress..."
nmap -sC $IpAddress

# Firewall detection
Write-Output "Performing firewall detection on $IpAddress..."
nmap -sA $IpAddress

# Traceroute
Write-Output "Performing traceroute on $IpAddress..."
nmap --traceroute $IpAddress

# MAC address lookup
Write-Output "Performing MAC address lookup on $IpAddress..."
nmap -sL $IpAddress

# DNS enumeration
Write-Output "Performing DNS enumeration on $IpAddress..."
nmap --dns-servers $IpAddress

# Vulnerability scan
Write-Output "Performing vulnerability scan on $IpAddress..."
nmap -sV --script vuln $IpAddress