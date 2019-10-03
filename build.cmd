if not exist .fake/fake.exe dotnet tool install --tool-path ".fake" fake-cli --add-source https://api.nuget.org/v3/index.json
.paket/paket.exe restore
.fake\fake.exe run build.fsx %*