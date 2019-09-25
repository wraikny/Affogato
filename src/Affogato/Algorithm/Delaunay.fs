namespace Affogato.Algorithm

open Affogato
open Affogato.Graph

module Delaunay2 =
    /// 全体を包含する正三角形を求める
    let getHugeTriangle (range : float32 Rectangle2) : float32 Triangle2 =

        // 与えられた矩形を包含する円を求める  
        let center = range |> Rectangle.centerPosition
        let radius = Vector.length (range.position - center)

        // その円に外接する正三角形を求める  
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


    let getTrianglesList (points : Node<float32 Vector2> list) : HashSet<TriangleEdge> =
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

        let range =
            {
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

        // 点を逐次添加し、反復的に三角分割を行う  
        for p in points do
            /// 追加候補の三角形を保持する一時ハッシュ
            /// Key : 三角形
            /// Value : 重複しているか
            let tmpTriangleSet = new Dictionary<TriangleEdge, bool>()

            let addToTmpSet tri =
                let tri = TriangleEdge(tri)

                let isDuplicated = tmpTriangleSet.ContainsKey(tri)
                if isDuplicated then
                    tmpTriangleSet.[tri] <- false
                else
                    tmpTriangleSet.Add(tri, true)


            // 現在の三角形リストから要素を一つずつ取り出して、  
            // 与えられた点が各々の三角形の外接円の中に含まれるかどうか判定  
            for t in (new HashSet<_>(trianglesSet)) do
                // 外接円
                let c = Triangle.circumscribedCircle t.Triangle

                let sqDistance = Vector.squaredLength (c.center - p.value)

                if(sqDistance < c.radius * c.radius) then
                    // 新しい三角形を作り、一時ハッシュに入れる  
                    let tri = t
                    addToTmpSet( (p, tri.Node1, tri.Node2) )
                    addToTmpSet( (p, tri.Node2, tri.Node3) )
                    addToTmpSet( (p, tri.Node3, tri.Node1) )
                    
                    // 旧い三角形を削除
                    trianglesSet.Remove(t) |> ignore

            
            // 一時ハッシュのうち、重複のないものを三角形リストに追加
            for pair in tmpTriangleSet do
                if pair.Value then
                    trianglesSet.Add(pair.Key) |> ignore


        let trianglesHavingCommonPointOfHuge =
            trianglesSet.Where(fun x ->
                Triangle.hasCommonPoint x.Triangle hugeTriangle
            ).ToList()

        
        for t in trianglesHavingCommonPointOfHuge do
            trianglesSet.Remove(t) |> ignore


        trianglesSet


    let getNodesList points : Edge<float32 Vector2, float32> list =
        let triangles = getTrianglesList points

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