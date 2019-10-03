module Affogato.Test.Easing
open Affogato
open Expecto

[<Tests>]
let easingTests =
  testList "Easing" [
    testProperty "Valid" <| fun e t ->
      isInvalid t
      || Easing.calculateF e t |> isInvalid |> not
  ]