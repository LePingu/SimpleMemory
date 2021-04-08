$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Write-host "My directory is $dir"
Push-Location $dir/..
docker build -t simple_memory_test -f ./SimpleMemory/Packaging/dockerfile.test .