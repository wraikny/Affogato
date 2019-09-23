[<AutoOpen>]
module internal Affogato.Support

let inline zero< ^a when ^a: (static member Zero: ^a) > = LanguagePrimitives.GenericZero< ^a >
let inline one< ^a when ^a: (static member One: ^a) > = LanguagePrimitives.GenericOne< ^a >

let inline map (f: 'a -> 'b) (x: ^``Functor<'a>``) : '``Functor<'b>`` =
  (^``Functor<'a>``: (static member Map: _*_->_) x, f)

let inline map2 (f: 'a -> 'b -> 'c) (x: ^``Functor<'a>``) (y: ^``Functor<'b>``) : '``Functor<'c>`` =
  ( (^``Functor<'a>`` or ^``Functor<'b>``): (static member Map2: _*_*_->_) x, y, f)

let inline pure' x: ^a = (^a: (static member Return: _->_) x)

let inline toSeq (x: ^a) =
  (^a : (static member ToSeq:_->seq<_>) x)