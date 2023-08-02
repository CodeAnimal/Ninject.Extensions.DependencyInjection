param(
    [Parameter(Mandatory=$true)]
    [string] $apiKey
)

dotnet nuget push "./nupkgs/*.nupkg" -k $apiKey -s https://api.nuget.org/v3/index.json --skip-duplicate
dotnet nuget push "./nupkgs/*.snupkg" -k $apiKey -s https://api.nuget.org/v3/index.json --skip-duplicate