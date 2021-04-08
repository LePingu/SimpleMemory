$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Write-host "My directory is $dir"
Push-Location $dir/..
Push-Location ./SimpleMemory
docker build -t simple_memory_image -f ./Packaging/dockerfile .
docker run --name simple_memory -p 5000:5000 -d simple_memory_image
