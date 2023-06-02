module Affogato.Test.Vector
open Affogato
open Expecto

[<Tests>]
let vector2Test =
  testList "Vector" [
    testList "Vector2" [
      testProperty "int Vector2 (+)" <| fun (a: int Vector2) b ->
        let c = a + b
        c.x = a.x + b.x && c.y = a.y + b.y
      testProperty "int Vector2 (-)" <| fun (a: int Vector2) b ->
        let c = a - b
        c.x = a.x - b.x && c.y = a.y - b.y
      testProperty "int Vector2 (*)" <| fun (a: int Vector2) b ->
        let c = a * b
        c.x = a.x * b.x && c.y = a.y * b.y
      testProperty "int Vector2 (/)" <| fun (a: int Vector2) (b: int Vector2) ->
        if b.x * b.y = 0 then true
        else
          let c = a / b
          c.x = a.x / b.x && c.y = a.y / b.y
      testProperty "int Vector2 Dot" <| fun (a: int Vector2) (b: int Vector2) ->
        Vector.dot a b = a.x * b.x + a.y * b.y
      testProperty "int Vector2 props" <| fun (a: int Vector2) ->
        a.xx + a.yy = a.xy + a.yx
      testProperty "int Vector2 toSeq" <| fun (a: int Vector2) ->
        a |> Vector.toSeq |> Seq.length = 2
      testProperty "int vector2 map" <| fun (a: int Vector2) d ->
        let b = a |> Vector.map ((+) d)
        b.x = a.x + d && b.y = a.y + d
      testCase "int Vetor2 fold" <| fun () ->
        Expect.equal
          (Vector2.init 1 2 |> Vector.fold (+) 0)
          3
          "fold"
      testCase "int Vetor2 forall" <| fun () ->
        Expect.isTrue
          (Vector.forall ((<) 0) (Vector2.init 1 2))
          "forall"
    ]

    testList "Vector3" [
      testProperty "int Vector3 (+" <| fun (a: int Vector3) b ->
        let c = a + b
        c.x = a.x + b.x && c.y = a.y + b.y && c.z = a.z + b.z
      testProperty "int Vector3 (-)" <| fun (a: int Vector3) b ->
        let c = a - b
        c.x = a.x - b.x && c.y = a.y - b.y && c.z = a.z - b.z
      testProperty "int Vector3 (*)" <| fun (a: int Vector3) b ->
        let c = a * b
        c.x = a.x * b.x && c.y = a.y * b.y && c.z = a.z * b.z
      testProperty "int Vector3 (/)" <| fun (a: int Vector3) (b: int Vector3) ->
        if b.x * b.y * b.z = 0 then true
        else
          let c = a / b
          c.x = a.x / b.x && c.y = a.y / b.y && c.z = a.z / b.z
      testProperty "int Vector3 Dot" <| fun (a: int Vector3) (b: int Vector3) ->
        Vector.dot a b = a.x * b.x + a.y * b.y + a.z * b.z
      testProperty "int Vector3 props" <| fun (a: int Vector3) ->
        a.xxx + a.yyy + a.zzz = a.xyz + a.yzx + a.zxy
      testProperty "int Vector3 toSeq" <| fun (a: int Vector3) ->
        a |> Vector.toSeq |> Seq.length = 3
      testProperty "int vector3 map" <| fun (a: int Vector3) d ->
        let b = a |> Vector.map ((+) d)
        b.x = a.x + d && b.y = a.y + d && b.z = a.z + d
      testCase "int Vetor3 fold" <| fun () ->
        Expect.equal
          (Vector3.init 1 2 3 |> Vector.fold (+) 0)
          6
          "fold"
      testCase "int Vetor3 forall" <| fun () ->
        Expect.isTrue
          (Vector.forall ((<) 0) (Vector3.init 1 2 3))
          "forall"
    ]

    testList "Vector4" [
      testProperty "int Vector4 (+" <| fun (a: int Vector4) b ->
        let c = a + b
        c.x = a.x + b.x && c.y = a.y + b.y && c.z = a.z + b.z && c.w = a.w + b.w
      testProperty "int Vector4 (-)" <| fun (a: int Vector4) b ->
        let c = a - b
        c.x = a.x - b.x && c.y = a.y - b.y && c.z = a.z - b.z && c.w = a.w - b.w
      testProperty "int Vector4 (*)" <| fun (a: int Vector4) b ->
        let c = a * b
        c.x = a.x * b.x && c.y = a.y * b.y && c.z = a.z * b.z && c.w = a.w * b.w
      testProperty "int Vector4 (/)" <| fun (a: int Vector4) (b: int Vector4) ->
        if b.x * b.y * b.z * b.w = 0 then true
        else
          let c = a / b
          c.x = a.x / b.x && c.y = a.y / b.y && c.z = a.z / b.z && c.w = a.w / b.w
      testProperty "int Vector4 Dot" <| fun (a: int Vector4) (b: int Vector4) ->
        Vector.dot a b = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w
      testProperty "int Vector4 props" <| fun (a: int Vector4) ->
        a.xxxx + a.yyyy + a.zzzz + a.wwww = a.xyzw + a.yzwx + a.zwxy + a.wxyz
      testProperty "int Vector4 toSeq" <| fun (a: int Vector4) ->
        a |> Vector.toSeq |> Seq.length = 4
      testProperty "int vector4 map" <| fun (a: int Vector4) d ->
        let b = a |> Vector.map ((+) d)
        b.x = a.x + d && b.y = a.y + d && b.z = a.z + d && b.w = a.w + d
      testCase "int Vetor4 fold" <| fun () ->
        Expect.equal
          (Vector4.init 1 2 3 4 |> Vector.fold (+) 0)
          10
          "fold"
      testCase "int Vetor4 forall" <| fun () ->
        Expect.isTrue
          (Vector.forall ((<) 0) (Vector4.init 1 2 3 4))
          "forall"
    ]
  ]