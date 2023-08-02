# Release Process
Build and package the project with the  _Release_ configuration.

```
$version = "1.0.2"
dotnet pack Ninject.Extensions.DependencyInjection.sln -c Release --output ./nupkgs /p:PackageVersion=$version /p:ContinuousIntegrationBuild=true
```

Publish all packages with the `publish.ps1` script, replacing the API key and versions as needed.
```
.\doc\publish.ps1 -apiKey "NuGet APIKey" -version "$version"
```

With the NuGet API key stored in the Windows credential manager, we can do
```
.\doc\publish.ps1 -apiKey $((Read-CredentialsStore -Target "NuGet:Ninject.Extensions.DependencyInjection:APIKey").GetNetworkCredential().Password)"
```
