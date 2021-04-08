$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Write-host "My directory is $dir"
Push-Location $dir/..
Push-Location ./SimpleMemory
dotnet build
dotnet run