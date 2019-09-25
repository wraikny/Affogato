module Affogato.Test.Easing
open Affogato
open Expecto

[<Tests>]
let easingTests =
  testList "Easing" [
    testProperty "Range" <| fun e t ->
      if isInvalid t then true
      else
        let x = Easing.calculateF e t
        0.0f <= x && x <= 1.0f
  ]