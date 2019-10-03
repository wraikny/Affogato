module Affogato.Test.Dungeon
open Affogato.Advanced.Dungeon
open Expecto

let inline builder seed = {
  seed = 100 + seed
  roomCount = 100
  roomGeneratedRange = 50.0f, 50.0f
  minRoomSize = 3, 3
  maxRoomSize = 6, 6
  roomMoveRate = 0.4f
  roomMeanThreshold = 1.2f
  restoreEdgeRate = 0.2f
  corridorWidth = 2
  displayLog = false
}

[<Tests>]
let tests =
  testList "Dungeon" [
    testCase "DungeonWithLog" <| fun () ->
      Builder.generate { builder 389 with displayLog = false }
      |> ignore
      ()
    testProperty "Result" <| fun (seed: uint16) ->
      let builder = builder (int seed)
      let m = Builder.generate builder
      let m' = Builder.generate builder

      m.largeRoomEdges.Length = m'.largeRoomEdges.Length
      && m.largeRooms.Count = m'.largeRooms.Count
      && m.smallRooms.Count = m'.smallRooms.Count
      && m.cells.Count = m'.cells.Count
      && m.corridors.Count = m'.corridors.Count

    testProperty "Limits" <| fun (seed: uint16) ->
      let builder = builder (int seed)
      
      let model = Builder.generate builder
      let lc, sc = model.largeRooms.Count, model.smallRooms.Count
      let result =
        (lc > 1 && sc > 1)
        && (lc+ sc <= builder.roomCount)
      if not result then
        printfn "%A\n%d %d" model lc sc


      result
  ]