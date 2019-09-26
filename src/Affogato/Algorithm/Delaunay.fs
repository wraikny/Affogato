namespace Affogato.Algorithm

open Affogato
open Affogato.Graph

module Delaunay2 =
  /// Find an equilateral triangle that encompasses the whole
  let inline private getHugeTriangle (range : float32 Rectangle2) : float32 Triangle2 =

    // Find a circle encompassing the given rectangle
    let center = range |> Rectangle.centerPosition
    let radius = Vector.length (range.position - center)

    // Find an equilateral triangle circumscribing the circle
    let sqrt3 = sqrt(3.0f)
    let bottom = center.y - radius

    let x1 = center.x - sqrt3 * radius
    let p1 = Vector2.init x1 bottom

    let x2 = center.x + sqrt3 * radius
    let p2 = Vector2.init x2 bottom

    let y3 = center.y + 2.0f * radius
    let p3 = Vector2.init center.x y3

    Triangle.init p1 p2 p3

  open System.Collections.Generic
  open System.Linq


  let getTriangles (points : Node<float32 Vector2> list) : HashSet<TriangleEdge> =
    let minX, minY, maxX, maxY =
      let rec f (ps : Node<float32 Vector2> list) result =
        ps |> function
        | [] -> result
        | {value = {x = x; y = y}}::ps ->
          let ix, iy, ax, ay = result

          let ix, ax = min x ix, max x ax
          let iy, ay = min y iy, max y ay

          f ps (ix, iy, ax, ay)

      f points (0.0f, 0.0f, 0.0f, 0.0f)

    let range = {
      position = Vector2.init minX minY
      size = Vector2.init (maxX - minX) (maxY - minY)
    }

    let hugeTriangle = getHugeTriangle range

    let trianglesSet = new HashSet<TriangleEdge>()

    let addTriangleToSet nodes =
      trianglesSet.Add( TriangleEdge(nodes) )

    let hugeTriangleNodes =
      let node1 = Node.init -1 hugeTriangle.p1
      let node2 = Node.init -2 hugeTriangle.p2
      let node3 = Node.init -3 hugeTriangle.p3

      (node1, node2, node3)

    addTriangleToSet(hugeTriangleNodes) |> ignore

    // Add points sequentially and repeat triangulation
    for p in points do
      /// Temporary hash holding additional candidate triangles
      /// Value : Is it duplicated?
      let tmpTriangleSet = new Dictionary<TriangleEdge, bool>()

      let addToTmpSet tri =
        let tri = TriangleEdge(tri)

        let isDuplicated = tmpTriangleSet.ContainsKey(tri)
        if isDuplicated then
          tmpTriangleSet.[tri] <- false
        else
          tmpTriangleSet.Add(tri, true)


      // Extract elements from the current triangle list one by one and determine
      // if the given point falls within the circumcircle of each triangle
      for t in (new HashSet<_>(trianglesSet)) do
        let c = Triangle.circumscribedCircle t.Triangle

        let sqDistance = Vector.squaredLength (c.center - p.value)

        if(sqDistance < c.radius * c.radius) then
          // Create a new triangle and place it in a temporary hash
          let tri = t
          addToTmpSet( (p, tri.Node1, tri.Node2) )
          addToTmpSet( (p, tri.Node2, tri.Node3) )
          addToTmpSet( (p, tri.Node3, tri.Node1) )

          // Remove old triangle
          trianglesSet.Remove(t) |> ignore

      // Add unique temporary hash to triangle list
      for pair in tmpTriangleSet.Where(fun x -> x.Value) do
        trianglesSet.Add(pair.Key) |> ignore


    let trianglesHavingCommonPointOfHuge = [
      for x in trianglesSet do
        if Triangle.hasCommonPoint x.Triangle hugeTriangle then
          yield x
    ]

    for t in trianglesHavingCommonPointOfHuge do
      trianglesSet.Remove(t) |> ignore

    trianglesSet


  let getEdges points : Edge<float32 Vector2, float32> list =
    let triangles = getTriangles points

    let edges = new List<Edge<float32 Vector2, float32>>()
    for t in triangles do
      let (e1, e2, e3) = t.Edges
      let addEdge e =
        if not <| edges.Exists(fun e0 -> Edge.equal e e0) then
          edges.Add(e)
      addEdge e1
      addEdge e2
      addEdge e3

    [for e in edges -> e ]
