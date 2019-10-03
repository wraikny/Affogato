namespace Affogato.Advanced.Dungeon

open Affogato
open Affogato.Graph
open Affogato.Algorithm
open Affogato.Collections

type Builder = {
  /// Random Seed
  seed : int

  /// Generating Rooms Count
  roomCount : int

  /// Generating Area Range
  roomGeneratedRange : float32 * float32

  /// Minimum Size of Genrating Rooms
  minRoomSize : int * int
  /// Maximum Size of Genrating Rooms
  maxRoomSize : int * int

  /// x << 1.0f
  roomMoveRate : float32

  /// Rate of Mean for deciding large rooms as threshold
  /// recommended: about 1.25f
  roomMeanThreshold : float32

  /// recommended: 0.08f ~ 0.15f
  restoreEdgeRate : float32

  /// Generating Corridors Width
  corridorWidth : int
}


type private WithRandom = {
    parameter : Builder
    random : System.Random
}

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module private WithRandom =
  let init (parameter : Builder) : WithRandom =
    {
      parameter = parameter
      random = System.Random(parameter.seed)
    }

  let getRandomValue (builder) =
    builder.random.NextDouble() |> float32

  let roomRectangles (builder : WithRandom) =
      let getRandomPointInCircle () : int Vector2 =
        let w, h = builder.parameter.roomGeneratedRange

        let t = 2.0f * Angle.PI * getRandomValue(builder)
        let u = getRandomValue(builder) * getRandomValue(builder)
        let r = if u > 1.0f then 2.0f - u else u

        (w * r * cos(t), h * r * sin(t))
        |> uncurry Vector2.init
        |>> int

      [ for _ in 1..builder.parameter.roomCount ->
        let pos = getRandomPointInCircle()

        let size =
          let min =
            builder.parameter.minRoomSize
            |> uncurry Vector2.init
            |>> float32

          let max =
            builder.parameter.maxRoomSize
            |> uncurry Vector2.init
            |>> float32

          let rand = builder |> getRandomValue

          (min + (max - min) .* rand)
          |>> int

        Rectangle.init (pos - size ./ 2) size
      ]


  let getLargeRoomEdges (largeRooms : seq<int * int Rectangle2>) (withRandom : WithRandom) : Edge<unit, float32> [] =
    let largeRooms = Seq.toArray largeRooms
    let largeRoomsCount = largeRooms |> Array.length
    
    let largeRoomEdges =
      largeRooms
      |> Seq.map(fun (id, rect) ->
        let pos = rect.position |>> float32
        let size = rect.size |>> float32
        Node.init id (pos + (size ./ 2.0f) )
      )
      |> Delaunay2.getEdges
    
    let largeRoomsSpanningTree =
      largeRoomEdges
      |> SpanningTree.Kruskal largeRoomsCount
    
    let largeRoomEdgesResult =
      [|
        yield! largeRoomsSpanningTree

        let exclusiondSpanningTree =
          largeRoomEdges
          |> Array.filter(fun e ->
            largeRoomsSpanningTree
            |> Seq.exists(fun e0 -> Edge.equal e e0)
            |> not
          )

        let restoreCount =
          let count = Seq.length exclusiondSpanningTree |> float32
          count * withRandom.parameter.restoreEdgeRate |> int

        yield!
          exclusiondSpanningTree
          |> Seq.sortBy(fun _ -> withRandom.random.NextDouble() )
          |> Seq.take restoreCount
      |]
    
    largeRoomEdgesResult
    |> Array.map (map ignore)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Builder =
    let private distributeRooms (rooms : seq<int Rectangle2>) (rate : float32) =
      let rooms = rooms |> Seq.toArray
      let threshold =
        let len = rooms |> Seq.length |> float32
        let sum : float32 Vector2 =
          rooms
          |> Seq.sumBy ( Rectangle.size >> (map float32) )
        sum .* (rate / len)

      rooms
      |> Array.partition(fun rect ->
        let size = rect.size |>> float32
        size.x >= threshold.x && size.y > threshold.y
      )

    open System.Collections.Generic

    let private moveRooms (rooms : int Rectangle2 list) movingRate : int Rectangle2 [] =
      let roomsList = List<MovingRoom>()

      for r in rooms do
        let movingRoom = MovingRoom(r |>> map float32, movingRate, roomsList)
        roomsList.Add(movingRoom)

      let mutable count = 1.0f
      while roomsList.Exists(fun r -> r.IsMoving) do
        count <- count + 1.0f

        for r in roomsList do r.Update(count)


      [| for r in roomsList -> r.RectI |]


    let private generateCorridors (width) (rect1 : int Rectangle2, rect2 : int Rectangle2) : int Rectangle2 [] =
      let center1, center2 = Rectangle.centerPosition rect1, Rectangle.centerPosition rect2
      let middle = (center1 + center2) ./ 2

      let lurd1 = rect1 |> Rectangle.lurd
      let lurd2 = rect2 |> Rectangle.lurd

      let isCollidedX = Rectangle.isCollidedAxis Vector.x lurd1 lurd2
      let isCollidedY = Rectangle.isCollidedAxis Vector.y lurd1 lurd2

      let manhattanDist = (center1 - center2) |>> abs

      let sizeDict =
        Vector2.init
          ( Vector2.init manhattanDist.x width )
          ( Vector2.init width manhattanDist.y )

      let createCorridorAt size pos =
        Rectangle.init (pos - size ./ 2) size


      if isCollidedX && isCollidedY then
        [||]
      elif isCollidedX then
        [| createCorridorAt sizeDict.y middle |]
      elif isCollidedY then
        [| createCorridorAt sizeDict.x middle |]
      else
        [|let f = createCorridorAt
          for center in [center1; center2 ] do
            yield f (sizeDict.x + Vector2.init width 0) ({ middle with y = center.y})
            yield f (sizeDict.y + Vector2.init 0 width) ({ middle with x = center.x})
        |]


    let private spacesToHashMap =
      Seq.map(fun (r : Space) ->
        r.id.Value, r
      )
      >> HashMap.ofSeq


    [<CompiledName "Generate">]
    let generate (builder : Builder) : Model =
      let withRandom = builder |> WithRandom.init

      let roomRectangles = withRandom |> WithRandom.roomRectangles

      let movedRooms = moveRooms roomRectangles builder.roomMoveRate

      let largeRoomRectangles, smallRoomRectangles =
        distributeRooms movedRooms builder.roomMeanThreshold

      let largeRoomRectanglesIndexed = largeRoomRectangles |> Array.indexed


      let largeRooms, smallRooms =
        let inline toRoom kind = Array.map <| fun (i, r) -> Space.init (kind i) r

        largeRoomRectanglesIndexed |> toRoom SpaceID.Large
        , smallRoomRectangles |> Array.indexed |> toRoom SpaceID.Small


      let largeRoomsHashMap = spacesToHashMap largeRooms


      let largeRoomsEdges =
        withRandom
        |> WithRandom.getLargeRoomEdges largeRoomRectanglesIndexed


      let corridorRectangles =
        [|
          for e in largeRoomsEdges do
            let room1 =
              largeRoomsHashMap
              |> HashMap.find e.node1.label

            let room2 =
              largeRoomsHashMap
              |> HashMap.find e.node2.label

            yield!
              generateCorridors builder.corridorWidth (room1.rect, room2.rect)
        |]

      let corridors =
        corridorRectangles
        |> Array.mapi (fun i r ->
          Space.init (SpaceID.Corridor i) r)


      let collidedSmallRooms =
        smallRooms
         |> Array.filter(fun room ->
           corridorRectangles
           |> Seq.exists(fun cr ->
               room.rect
               |> Rectangle.isCollided2 cr
           )
         )

      let cellsMap =
        let inline getCells (spaces : seq<Space>) =
            Seq.collect Space.cells spaces

        seq {
          let cellsDict = new Dictionary<int Vector2, SpaceID>()

          for (cdn, id) in (getCells largeRooms) do
            cellsDict.[cdn] <- id

          for (cdn, id) in (getCells collidedSmallRooms) do
            if not <| cellsDict.ContainsKey(cdn) then
              cellsDict.[cdn] <- id

          for (cdn, id) in (getCells corridors) do
            if not <| cellsDict.ContainsKey(cdn) then
              cellsDict.[cdn] <- id

          for item in cellsDict ->
            (item.Key, item.Value)
        }
        |> HashMap.ofSeq


      {
        largeRooms = largeRoomsHashMap
        smallRooms = collidedSmallRooms |> spacesToHashMap
        corridors = corridors |> spacesToHashMap

        largeRoomEdges = largeRoomsEdges

        cells = cellsMap
      }