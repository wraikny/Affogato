
module Affogato.Helper

#nowarn "0064"

open System

let inline implicit (x: ^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

let inline dispose(x: IDisposable) = if x <> null then x.Dispose()

let inline uncurry f (x, y) = f x y
let inline flip f x y = f y x
let inline devidedBy q p = p / q


let inline isNan x = System.Double.IsNaN (float x)
let inline isInfinity x = System.Double.IsInfinity (float x)

let inline isInvalid x = isNan x || isInfinity x

type Builtin = Builtin
with
  static member inline Map(x, f) = List.map f x
  static member inline Map(x, f) = Array.map f x
  static member inline Map(x, f) = Map.map f x
  static member inline Map(x, f) = Set.map f x
  static member inline Map(x, f) = Option.map f x
  static member inline Map(x, f) = ValueOption.map f x
  static member inline Map(x, f) = Result.map f x

  static member inline Map2(x, y, f) = List.map2 f x y
  static member inline Map2(x, y, f) = Array.map2 f x y
  static member inline Map2(x, y, f) = Option.map2 f x y
  static member inline Map2(x, y, f) = ValueOption.map2 f x y

  static member inline ToSeq(x) = List.toSeq x
  static member inline ToSeq(x) = Array.toSeq x
  static member inline ToSeq(x) = Map.toSeq x
  static member inline ToSeq(x) = Set.toSeq x
  static member inline ToSeq(x) = x |> function | Some a -> seq { yield a } | _ ->  Seq.empty
  static member inline ToSeq(x) = x |> function | ValueSome a -> seq { yield a } | _ ->  Seq.empty

  static member inline Iter(x, f) = List.iter f x
  static member inline Iter(x, f) = Array.iter f x
  static member inline Iter(x, f) = Map.iter f x
  static member inline Iter(x, f) = Set.iter f x
  static member inline Iter(x, f) = Option.iter f x
  static member inline Iter(x, f) = ValueOption.iter f x

  static member inline Exists(x, f) = List.exists f x
  static member inline Exists(x, f) = Array.exists f x
  static member inline Exists(x, f) = Map.exists f x
  static member inline Exists(x, f) = Set.exists f x
  static member inline Exists(x, f) = Option.exists f x
  static member inline Exists(x, f) = ValueOption.exists f x

  static member inline ForAll(x, f) = List.forall f x
  static member inline ForAll(x, f) = Array.forall f x
  static member inline ForAll(x, f) = Map.forall f x
  static member inline ForAll(x, f) = Set.forall f x
  static member inline ForAll(x, f) = Option.forall f x
  static member inline ForAll(x, f) = ValueOption.forall f x


let inline zero< ^a when ^a: (static member Zero: ^a) > = LanguagePrimitives.GenericZero< ^a >
let inline one< ^a when ^a: (static member One: ^a) > = LanguagePrimitives.GenericOne< ^a >

let inline pure' x: ^a = (^a: (static member Return: _->_) x)


let inline map (f: ^a -> ^b) (x: ^``Functor<'a>``) : ^``Functor<'b>`` =
  let inline map (_: ^Builtin) f (x: ^``Functor<'a>``) =
    ( (^``Functor<'a>`` or ^Builtin): (static member Map: _*_->_) x, f )
  map Builtin f x

let inline (|>>) (x: ^``Functor<'a>``) (f: ^a -> ^b) : ^``Functor<'b>`` = map f x

let inline map2 (f: ^a -> ^b -> ^c) (x: ^``Functor<'a>``) (y: ^``Functor<'b>``) : ^``Functor<'c>`` =
  let inline f' (_: ^Builtin) f (x: ^``Functor<'a>``) (y: ^``Functor<'b>``)  =
    ( (^``Functor<'a>`` or ^``Functor<'b>`` or ^Builtin): (static member Map2: _*_*_->_) x,y,f )
  f' Builtin f x y

let inline map2' (f: ^a -> ^a -> ^a) (x: ^``Functor<'a>``) (y: ^``Functor<'a>``): ^``Functor<'a>`` =
  map2 f x y

    
let inline toSeq (x: ^``Foldable<'a>``) :_ =
  let inline f' (_: ^Builtin) (x: ^``Foldable<'a>``) =
    ((^``Foldable<'a>`` or ^Builtin): (static member ToSeq:_->seq<_>) x)
  f' Builtin x

let inline toList (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toList
let inline toArray (x: ^``Foldable<'a>``) = x |> toSeq |> Seq.toArray
let inline toMap (x: ^``Foldable<'a>``) = x |> toSeq |> Map.ofSeq
let inline toSet (x: ^``Foldable<'a>``) = x |> toSeq |> Set.ofSeq

let inline iter f (x: ^``Foldable<'a>``) =
  let inline f' (_: ^Builtin) f (x: ^``Foldable<'a>``) =
    ((^``Foldable<'a>`` or ^Builtin): (static member Iter:_*_->unit) x, f)
  f' Builtin f x

let inline exists f (x: ^``Foldable<'a>``) =
  let inline f' (_: ^Builtin) f (x: ^``Foldable<'a>``) =
    ((^``Foldable<'a>`` or ^Builtin): (static member Exists:_*_->bool) x, f)
  f' Builtin f x
  
let inline forall f (x: ^``Foldable<'a>``) =
  let inline f' (_: ^Builtin) f (x: ^``Foldable<'a>``) =
    ((^``Foldable<'a>`` or ^Builtin): (static member ForAll:_*_->bool) x, f)
  f' Builtin f x

type Builtin with
  static member inline ToSeq(x: seq<_>) = x

module String =
  let inline toLower (s: string) = s.ToLower()
  let inline toUpper (s: string) = s.ToUpper()
