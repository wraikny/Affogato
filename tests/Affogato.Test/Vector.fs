module Affogato.Test.Vector
open Affogato
open Expecto

[<Tests>]
let vector2Test =
  testList "Vector" [
    testList "Vector2" [
      testProperty "int Vector2 (+" <| fun (a: int Vector2) b ->
        let c = a + b
        c.x = a.x + b.x && c.y = a.y + b.y
      testProperty "int Vector2 (-)" <| fun (a: int Vector2) b ->
        let c = a - b
        c.x = a.x - b.x && c.y = a.y - b.y
      testProperty "int Vector2 (*)" <| fun (a: int Vector2) b ->
        let c = a * b
        c.x = a.x * b.x && c.y = a.y * b.y
      testProperty "int Vector2 (/)" <| fun (a: int Vector2) (b: int Vector2) ->
        if b.x = 0 || b.y = 0 then true
        else
          let c = a / b
          c.x = a.x / b.x && c.y = a.y / b.y
      testProperty "int Vector2 Dot" <| fun (a: int Vector2) (b: int Vector2) ->
        Vector.dot a b = a.x * b.x + a.y * b.y
    ]
  ]