module Affogato.Test.Dungeon
open Affogato.Advanced.Dungeon
open Expecto

let tests =
  testList "Dungeon" [
    testProperty "Run" <| fun seed (count: uint16) range  ->
      let builder = {
        seed = seed
        roomCount = 1 + int count
        roomGeneratedRange = 100.0f, 100.0f
        minRoomSize = 4, 4
        maxRoomSize = 8, 8
        roomMoveRate = 0.1f
        roomMeanThreshold = 1.25f
        restoreEdgeRate = 0.1f
        corridorWidth = 3
      }
      try
        let model = Builder.generate builder
        let lc, sc = model.largeRooms.Count , model.smallRooms.Count
        (lc+ sc <= builder.roomCount)
        && (lc > 0 && sc > 0)
      with _ ->
        false
  ]