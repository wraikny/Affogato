#!/bin/bash

if [ -e ./dist]; then
    rm ./dist/*
else
    mkdir ./dist
fi

cp ./output/Release/netstandard2.0/*.dll ./dist/.
cp ./output/Release/netstandard2.0/*.pdb ./dist/.
zip ./dist/Affogato.zip -r ./dist

if [ ! -e ./output/Relese ]; then
    ./fake.sh build
fi

./fake.sh build -t "Pack"
