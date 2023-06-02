module Affogato.Test.Rectangle
open Affogato
open Expecto

[<Tests>]
let rectangletest =
  testList "Rectangle" [
    testList "Rectangle2" [
      testCase "isInside" (fun () ->
        let pos = Vector2.init 0f 0f
        let size = Vector2.init 1f 1f

        let rect = Rectangle.init pos size
        Expect.isTrue (Rectangle.isInside (pos + size ./ 2f) rect)
          "inside"
      )

      testList "collided" ([
        yield (-1, -1)
        yield (1, 1)
        yield (-1, 0)
        yield (2, 2)
        for i in -2..2 do
          yield (-4, i)
      ] |> List.distinct |> List.map (fun (bx, by) ->
        testCase (sprintf "(%d, %d)" bx by) (fun () ->
          let a = Rectangle.init Vector.zero (Vector2.init 2 2)
          let b = Rectangle.init (Vector2.init bx by) (Vector2.init 4 2)
          Expect.isTrue (Rectangle.isCollided a b) "must be collided"
        )
      ))

      testList "not collided" ([
        for i in -3..3 do
          yield (i, 3)
          yield (3, i)
          yield (i, -3)
          yield (-3, i)
      ] |> List.distinct |> List.map (fun (bx, by) ->
        testCase (sprintf "(%d, %d)" bx by) (fun () ->
          let size = Vector2.init 2 2
          let a = Rectangle.init Vector.zero size
          let b = Rectangle.init (Vector2.init bx by) size
          Expect.isFalse (Rectangle.isCollided a b) "must not be collided"
        )
      ))
    ]
  ]
