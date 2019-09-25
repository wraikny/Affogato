module Affogato.Test.Vector
open Affogato
open Expecto

[<Tests>]
let vector2Test =
  testList "Vector" [
    testList "Vector2" [
      testProperty "Vector2 Add" <| fun (a: int Vector2) b ->
        let c = a + b
        c.x = a.x + b.x && c.y = a.y + b.y
      testProperty "Vector2 Sub" <| fun (a: int Vector2) b ->
        let c = a - b
        c.x = a.x - b.x && c.y = a.y - b.y
    ]
  ]