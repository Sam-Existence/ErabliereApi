function Get-IsWindowsOS() {
    $os = [System.Environment]::OSVersion.Platform;
    if ($os -eq "Win32NT") {
        return $true;
    }
    return $false;
}

function ConvertTo-CrossPlatformPath([string] $path) {

    if ($false -eq (Get-IsWindowsOS)) {
        $path = $path -replace "\", "/";
        return $path;
    }

    return $path;
}