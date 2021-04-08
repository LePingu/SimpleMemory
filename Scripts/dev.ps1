$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Write-host "My directory is $dir"
Push-Location $dir/..
Push-Location ./SimpleMemory.Tests
dotnet build
dotnet test