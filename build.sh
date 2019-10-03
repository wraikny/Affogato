#!/bin/bash
mono .paket/paket.exe restore
./fake.sh run build.fsx $@