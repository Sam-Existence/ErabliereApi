function Get-MyCertificat([string]$certificateName) {
    # Check if the OS is windows
    $os = [System.Environment]::OSVersion.Platform;
    if ($os -eq "Win32NT") {
        return Get-ChildItem -Path Cert:\LocalMachine\My -Recurse | Where-Object {$_.Subject -eq "CN=$certificateName"}
    } else {
        # Find de certificate in a linux environment
        return Get-ChildItem -Path /etc/ssl/certs -Recurse | Where-Object {$_.Name.StartsWith($certificateName)}
    }
}