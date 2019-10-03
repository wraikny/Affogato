module Affogato.Test.Easing
open Affogato
open Expecto

[<Tests>]
let easingTests =
  testList "Easing" [
    testProperty "Run" <| fun e t ->
      if isInvalid t then true
      else
        let x = Easing.calculateF e t
        true
  ]