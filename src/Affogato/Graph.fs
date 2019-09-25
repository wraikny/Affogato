namespace Affogato.Graph

open Affogato

[<CustomEquality; NoComparison>]
type Node<'a> = {
    label : int
    value : 'a
} with
  static member inline Map(node, f) =
    { label = node.label; value = f node.value }

  override this.Equals(other) =
    match other with
    | :? Node<'a> as other ->
      this.label = other.label
    | _ -> false

  override this.GetHashCode() =
    this.label.GetHashCode()


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Node =
  let inline init label value = { label=label; value=value }


[<CustomEquality; NoComparison>]
type Edge< 'a, 'w
  when 'a: struct
  and  'w: equality
  > = {
    node1 : Node<'a>
    node2 : Node<'a>
    weight : 'w
} with
  static member inline Map(edge, f) =
    { weight = edge.weight
      node1 = edge.node1 |>> f
      node2 = edge.node2 |>> f
    }

  override this.Equals(other) =
    match other with
    | :? Edge<'a,'w> as other ->
      (this.node1.label = other.node1.label)
      && (this.node2.label = other.node2.label)
      && (this.weight = other.weight)
    | _ -> false

  override this.GetHashCode() =
    this.node1.GetHashCode()
    ^^^ this.node2.GetHashCode()
    ^^^ this.weight.GetHashCode()


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Edge =
  let inline label1 x = x.node1.label
  let inline label2 x = x.node2.label
  let inline weight x = x.weight

  let inline init a b w = { node1 = a; node2 = b; weight = w }

  let equal (e1 : Edge<'a, 'b>) (e2 : Edge<'a, 'b>) =
    (e1.node1 = e2.node1 && e1.node2 = e2.node2)
    || (e1.node1 = e2.node2 && e1.node2 = e2.node1)



type TriangleEdge(node1: Node<Vector2<float32>>, node2, node3) =
  let triangle = Triangle.init node1.value node2.value node3.value
  let labelTriangle = Triangle.init node1.label node2.label node3.label

  let edges =
    let inline createEdge a b =
      let w = Vector.length(a.value - b.value)
      Edge.init a b w

    let e1 = createEdge node1 node2
    let e2 = createEdge node2 node3
    let e3 = createEdge node3 node1
    (e1, e2, e3)

  member val Node1 = node1 with get
  member val Node2 = node2 with get
  member val Node3 = node3 with get

  member val Triangle = triangle with get
  member val LabelTriangle = labelTriangle with get
  member val Edges = edges with get

  override this.GetHashCode() =
    node1.label.GetHashCode()
    ^^^ node2.label.GetHashCode()
    ^^^ node3.label.GetHashCode()
  
  override this.Equals(o) =
    o |> function
    | :? TriangleEdge as other ->
        Triangle.equal this.LabelTriangle other.LabelTriangle
    | _ -> false