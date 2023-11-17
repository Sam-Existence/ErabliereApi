# For each .csproj file in the current directory, update the version number

$version = ".net8.0"

$files = Get-ChildItem -Filter *.csproj -Recurse

foreach ($file in $files) {
    $xml = [xml](Get-Content $file)
    $xml.Project.PropertyGroup.TargetFramework = $version
    $xml.Project.PropertyGroup.Version = "3.1.0"
    $xml.Save($file.FullName)
}
