# Cspdf NuGet Package Publishing Script
# PowerShell script for publishing to NuGet

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey
)

Write-Host "Building and packing Cspdf..." -ForegroundColor Green

# Build in Release mode
dotnet build --configuration Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Pack the project
dotnet pack --configuration Release --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Pack failed!" -ForegroundColor Red
    exit 1
}

# Find the nupkg file
$nupkgFile = Get-ChildItem -Path "bin\Release" -Filter "*.nupkg" | Select-Object -First 1

if (-not $nupkgFile) {
    Write-Host "NuGet package not found!" -ForegroundColor Red
    exit 1
}

Write-Host "Found package: $($nupkgFile.FullName)" -ForegroundColor Green
Write-Host "Publishing to NuGet.org..." -ForegroundColor Green

# Publish to NuGet
dotnet nuget push $nupkgFile.FullName --api-key $ApiKey --source https://api.nuget.org/v3/index.json

if ($LASTEXITCODE -eq 0) {
    Write-Host "Successfully published to NuGet.org!" -ForegroundColor Green
} else {
    Write-Host "Publishing failed!" -ForegroundColor Red
    exit 1
}


