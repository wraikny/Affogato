[<AutoOpen>]
module Affogato.Support

#nowarn "0064"

let inline implicit (x: ^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

let inline zero< ^a when ^a: (static member Zero: ^a) > = LanguagePrimitives.GenericZero< ^a >
let inline one< ^a when ^a: (static member One: ^a) > = LanguagePrimitives.GenericOne< ^a >

let inline uncurry f (x, y) = f x y
let inline flip f x y = f y x
let inline devidedBy q p = p / q

type FunctorBuiltin = FunctorBuiltin
with
  static member inline Map(x, f) = List.map f x
  static member inline Map(x, f) = Array.map f x
  static member inline Map(x, f) = Array2D.map f x
  static member inline Map(x, f) = Array3D.map f x
  static member inline Map(x, f) = Map.map f x
  static member inline Map(x, f) = Set.map f x
  // static member inline Map(x: seq<_>, f) = Seq.map f x


let inline map (f: ^a -> ^b) (x: ^``Functor<'a>``) : ^``Functor<'b>`` =
  let inline map (_: ^Builtin) f (x: ^``Functor<'a>``) =
    ( (^``Functor<'a>`` or ^Builtin): (static member Map: _*_->_) x, f )
  map FunctorBuiltin f x

let inline (|>>) (x: ^``Functor<'a>``) (f: ^a -> ^b) : ^``Functor<'b>`` = map f x

let inline map2 (f: ^a -> ^b -> ^c) (x: ^``Functor<'a>``) (y: ^``Functor<'b>``) : ^``Functor<'c>`` =
  ( (^``Functor<'a>`` or ^``Functor<'b>``): (static member Map2: _*_*_->_) x, y, f)

let inline map2' (f: ^a -> ^a -> ^a) (x: ^``Functor<'a>``) (y: ^``Functor<'a>``): ^``Functor<'a>`` =
  map2 f x y

let inline pure' x: ^a = (^a: (static member Return: _->_) x)

let inline isNan x = System.Double.IsNaN (float x)
let inline isInfinity x = System.Double.IsInfinity (float x)

let inline isInvalid x = isNan x || isInfinity x


[<AutoOpen>]
module Foldable =
  type FoldableBuiltin = FoldableBuiltin
  with
    static member inline ToSeq(x) = List.toSeq x
    static member inline ToSeq(x) = Array.toSeq x
    static member inline ToSeq(x) = Map.toSeq x
    static member inline ToSeq(x) = Set.toSeq x
    static member inline ToSeq(x: seq<_>) = x
    
  let inline toSeq (x: ^``Foldable<'a>``) =
    let inline toSeq (_: ^Builtin) (x: ^``Foldable<'a>``) =
      ((^``Foldable<'a>`` or ^Builtin): (static member ToSeq:_->seq<_>) x)
    toSeq FoldableBuiltin x

  let inline toArray (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toArray
  let inline toList (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toList
  let inline toMap (x: ^``Foldable<'a>``) = x |> toSeq |> Map.ofSeq
  let inline toSet (x: ^``Foldable<'a>``) = x |> toSeq |> Set.ofSeq

  let inline exist f (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.exists f
  let inline forall f (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.forall f