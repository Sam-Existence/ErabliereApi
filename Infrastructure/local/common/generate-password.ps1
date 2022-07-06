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