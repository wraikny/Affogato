namespace Affogato.Advanced.Dungeon

open Affogato
open Affogato.Helper
open Affogato.Graph
open Affogato.Collections

[<Struct>]
type SpaceID =
  | Large of large:int
  | Small of small:int
  | Corridor of corridor:int
with
  member inline x.Value = x |> function
    | Large id
    | Small id
    | Corridor id -> id


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SpaceID =
  let inline value (x: SpaceID) = x.Value


[<Struct>]
type Space = {
  id : SpaceID
  rect : int Rectangle2
}


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Space =
  let inline internal init id rect = {
    id = id
    rect = rect
  }

  let inline id s = s.id

  let inline rect s = s.rect

  let inline cellsFor (size : int Vector2) (space) =
    let lu = space.rect.position
    let spaceSize = space.rect.size
    [|
      for dx in 0..(spaceSize.x - size.x) do
      for dy in 0..(spaceSize.y - size.y) do
        yield ( Vector2.init (lu.x + dx) (lu.y + dy), space.id )
    |]

  let inline cells space =
    cellsFor one space


type Model = {
  /// Key: ID
  largeRooms : HashMap<int, Space>
  /// Key: ID
  smallRooms : HashMap<int, Space>
  /// Key: ID
  corridors : HashMap<int, Space>

  /// Label: LargeRoomsID, Weight: distance
  largeRoomEdges : Edge<unit, float32> []

  /// id of each cell coordinate
  cells : HashMap<int Vector2, SpaceID>
}


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Model =
  let inline tryFindSpace id dungeon =
    let target, id = id |> function
      | Large id -> dungeon.largeRooms, id
      | Small id -> dungeon.smallRooms, id
      | Corridor id -> dungeon.corridors, id

    target
    |> HashMap.tryFind id


  let getSpaceAt coordinate dungeon =
    dungeon.cells
    |> HashMap.tryFind coordinate
    |> Option.bind (flip tryFindSpace dungeon)


  let inline cellToCoordinate (cellSize : float32 Vector2) (cell : int Vector2) : float32 Vector2 =
    let cellf = cell |>> float32
    cellf * cellSize


  let inline coordinateToCell (cellSize : float32 Vector2) (coordinate : float32 Vector2) : int Vector2 =
    coordinate / cellSize
    |>> (floor >> int)