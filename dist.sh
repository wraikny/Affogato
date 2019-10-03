#!/bin/bash
rm -r ./dist
mkdir ./dist
cp ./output/Release/netstandard2.0/*.dll ./dist/.
cp ./output/Release/netstandard2.0/*.pdb ./dist/.
zip ./dist/Affogato.zip -r ./dist