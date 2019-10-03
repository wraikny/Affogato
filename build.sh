#!/bin/bash
if [ ! -f .fake/fake ]; then
    dotnet tool install --tool-path ".fake" fake-cli --add-source https://api.nuget.org/v3/index.json
fi
mono .paket/paket.exe restore
.fake/fake run build.fsx $@