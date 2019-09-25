namespace Affogato.Algorithm

open System.Collections.Generic
open Affogato
open Affogato.Graph


type SpanningTree =
  static member inline Kruskal nodeCount (edges : seq<Edge<_,_>>): _ list =
    
    let uf = UnionFold(nodeCount)

    let sortedEdges =
      edges
      |> Seq.sortBy Edge.weight

    let resultEdges = List<_>()

    for edge in sortedEdges do
      let label1 = edge |> Edge.label1
      let label2 = edge |> Edge.label2
      if not <| uf.InSameSet(label1, label2) then
        uf.Unite(label1, label2)
        resultEdges.Add(edge)

    [ for edge in resultEdges -> edge ]

