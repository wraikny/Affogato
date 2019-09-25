[<AutoOpen>]
module Affogato.Support

let inline implicit (x: ^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

let inline zero< ^a when ^a: (static member Zero: ^a) > = LanguagePrimitives.GenericZero< ^a >
let inline one< ^a when ^a: (static member One: ^a) > = LanguagePrimitives.GenericOne< ^a >

let inline flip f x y = f y x
let inline devidedBy q p = p / q

let inline map (f: ^a -> ^b) (x: ^``Functor<'a>``) : ^``Functor<'b>`` =
  (^``Functor<'a>``: (static member Map: _*_->_) x, f)

let inline (|>>) (x: ^``Functor<'a>``) (f: ^a -> ^b) : ^``Functor<'b>`` = map f x

let inline map2 (f: ^a -> ^b -> ^c) (x: ^``Functor<'a>``) (y: ^``Functor<'b>``) : ^``Functor<'c>`` =
  ( (^``Functor<'a>`` or ^``Functor<'b>``): (static member Map2: _*_*_->_) x, y, f)

let inline pure' x: ^a = (^a: (static member Return: _->_) x)

let inline isNan x = System.Double.IsNaN (float x)
let inline isInfinity x = System.Double.IsInfinity (float x)

let inline isInvalid x = isNan x || isInfinity x

[<AutoOpen>]
module Foldable =
  let inline toSeq (x: ^``Foldable<'a>``) =
    (^``Foldable<'a>`` : (static member ToSeq:_->seq<_>) x)

  let inline toArray (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toArray
  let inline toList (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toList
  let inline toMap (x: ^``Foldable<'a>``) = x |> toSeq |> Map.ofSeq
  let inline toSet (x: ^``Foldable<'a>``) = x |> toSeq |> Set.ofSeq

  let inline exist f (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.exists f
  let inline forall f (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.forall f