namespace Affogato.Algorithm

open Affogato.Graph
open System.Linq

type UnionFold(num) =
  let par = (seq { for i in 0..num -> i }).ToList()
  let rank = (seq { for _ in 0..num -> 0 }).ToList()

  member inline private this.Root (node : int) =
    if par.[node] = node then
      node
    else
      let r = this.Root(par.[node])
      par.[node] <- r
      r


  member inline this.InSameSet(label1, label2) =
    this.Root(label1) = this.Root(label2)

  member inline this.InSameSet(edge: Edge<_,_>) =
    this.InSameSet(edge.node1.label, edge.node2.label)


  member this.Unite(label1, label2) =
    let x = this.Root(label1)
    let y = this.Root(label2)

    if x = y then ()
    elif rank.[x] < rank.[y] then
      par.[x] <- y
    else
      par.[y] <- x
      if rank.[x] = rank.[y] then
        rank.[x] <- rank.[x] + 1

  member this.Unite(edge: Edge<_,_>) =
    this.Unite(edge.node1.label, edge.node2.label)