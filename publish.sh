#!/bin/bash
# Cspdf NuGet Package Publishing Script
# Bash script for publishing to NuGet

if [ -z "$1" ]; then
    echo "Usage: ./publish.sh YOUR_API_KEY"
    exit 1
fi

API_KEY=$1

echo "Building and packing Cspdf..."

# Build in Release mode
dotnet build --configuration Release

if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

# Pack the project
dotnet pack --configuration Release --no-build

if [ $? -ne 0 ]; then
    echo "Pack failed!"
    exit 1
fi

# Find the nupkg file
NUPKG_FILE=$(find bin/Release -name "*.nupkg" | head -n 1)

if [ -z "$NUPKG_FILE" ]; then
    echo "NuGet package not found!"
    exit 1
fi

echo "Found package: $NUPKG_FILE"
echo "Publishing to NuGet.org..."

# Publish to NuGet
dotnet nuget push "$NUPKG_FILE" --api-key "$API_KEY" --source https://api.nuget.org/v3/index.json

if [ $? -eq 0 ]; then
    echo "Successfully published to NuGet.org!"
else
    echo "Publishing failed!"
    exit 1
fi


