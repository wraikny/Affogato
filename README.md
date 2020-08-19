[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fwraikny%2FAffogato.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fwraikny%2FAffogato?ref=badge_shield)
[![](https://github.com/wraikny/Affogato/workflows/CI/badge.svg)](https://github.com/wraikny/Affogato/actions?query=workflow%3ACI)
[![Nuget](https://img.shields.io/nuget/v/Affogato?style=plastic)](https://www.nuget.org/packages/Affogato/)
# Affogato

## Description
Affgato is a library of F# generic math types for game programming.  
Affgato.Advanced provides several practical algorithms.
This is stil in preview now.

## Example
```FSharp
open Affogato
let v1 = Vector2.init 1 0 // int Vector2
let v2 = Vector2.init 2.1f 3.4f // float32 Vector2
let v3 = v1 |> Vector.map float32 // float32 Vector2
let v4 = v2 / v3 .* 2.0f * v2.yy + v3.xx // float32 Vector2
let r1 = Rectangle.init v3 v4 // float32 Vector2 Rectangle
let k = Vector.dot v2 v3 // float32
```

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fwraikny%2FAffogato.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fwraikny%2FAffogato?ref=badge_large)