namespace Affogato

open Affogato.Helper

#nowarn "0064"

[<Struct>]
type Vector2< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = {
  x : ^a
  y : ^a
} with
  static member inline Init(x, y) = { x = x; y = y }
  static member inline Return(k : ^a) = Vector2< ^a >.Init(k, k)
  static member inline Zero: ^a Vector2 = pure' zero
  static member inline One: ^a Vector2 = pure' one

  static member inline Map(v: ^a Vector2, f: ^a -> ^b)=
    Vector2< ^b >.Init(f v.x, f v.y)

  static member inline Map2(a: ^a Vector2, b: ^b Vector2, f): ^c Vector2 ={
    x = f a.x b.x
    y = f a.y b.y
  }

  static member inline (~-) (a: 't Vector2): 't Vector2 = map (~-) a
  static member inline (+) (a, b): 't Vector2 = map2' (+) a b
  static member inline (-) (a, b): 't Vector2 = map2' (-) a b
  static member inline (*) (a, b): 't Vector2 = map2' (*) a b
  static member inline (/) (a, b): 't Vector2 = map2' (/) a b
  static member inline (%) (a, b): 't Vector2 = map2' (%) a b
  static member inline ( *. ) (a: 't, b: 't Vector2) = pure' a * b
  static member inline ( /. ) (a: 't, b: 't Vector2) = pure' a / b
  static member inline ( .* ) (a: 't Vector2, b: 't) = a * pure' b
  static member inline ( ./ ) (a: 't Vector2, b: 't) = a / pure' b
  static member inline ( .% ) (a: 't Vector2, b: 't) = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y

  static member inline ToSeq (v : ^a Vector2) =
    seq { yield v.x; yield v.y }

  static member inline Iter (v: ^a Vector2, f: ^a -> unit) = f v.x; f v.y
  static member inline Exists (v: ^a Vector2, f: ^a -> bool) = f v.x || f v.y
  static member inline ForAll (v: ^a Vector2, f: ^a -> bool) = f v.x && f v.y
  static member inline Iter2 (a: ^a Vector2, b: ^b Vector2, f: ^a -> ^b -> unit) = f a.x b.x; f a.y b.y;
  static member inline Exists2 (a: ^a Vector2, b: ^b Vector2, f: ^a -> ^b -> bool) = f a.x b.x || f a.y b.y
  static member inline ForAll2 (a: ^a Vector2, b: ^b Vector2, f: ^a -> ^b -> bool) = f a.x b.x && f a.y b.y

  static member inline Iter' (f: (^a Vector2 -> ^a) -> unit) =
    f (fun v -> v.x)
    f (fun v -> v.y)
  static member inline Exists' (f: (^a Vector2 -> ^a) -> bool) =
    f (fun v -> v.x) || f (fun v -> v.y)
  static member inline ForAll' (f: (^a Vector2 -> ^a) -> bool) =
    f (fun v -> v.x) && f (fun v -> v.y)

[<Struct>]
type Vector3< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = {
  x : ^a
  y : ^a
  z : ^a
} with
  static member inline Init(x, y, z) = { x = x; y = y; z = z }
  static member inline Return(k : ^a) = Vector3< ^a >.Init(k, k, k)
  static member inline Zero: ^a Vector3 = pure' zero
  static member inline One: ^a Vector3 = pure' one

  static member inline Map(v: ^a Vector3, f: ^a -> ^b)=
    Vector3< ^a >.Init(f v.x, f v.y, f v.z)

  static member inline Map2(a, b, f) = {
    x = f a.x b.x
    y = f a.y b.y
    z = f a.z b.z
  }

  static member inline (~-) (a: 't Vector3): 't Vector3 = map (~-) a
  static member inline (+) (a, b): 't Vector3 = map2' (+) a b
  static member inline (-) (a, b): 't Vector3 = map2' (-) a b
  static member inline (*) (a, b): 't Vector3 = map2' (*) a b
  static member inline (/) (a, b): 't Vector3 = map2' (/) a b
  static member inline (%) (a, b): 't Vector3 = map2' (%) a b
  static member inline ( *. ) (a: 't, b: 't Vector3) = pure' a * b
  static member inline ( /. ) (a: 't, b: 't Vector3) = pure' a / b
  static member inline ( .* ) (a: 't Vector3, b: 't) = a * pure' b
  static member inline ( ./ ) (a: 't Vector3, b: 't) = a / pure' b
  static member inline ( .% ) (a: 't Vector3, b: 't) = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y + a.z * b.z

  static member inline ToSeq (v : ^a Vector3) =
    seq { yield v.x; yield v.y; yield v.z }

  static member inline Iter (v: ^a Vector3, f: ^a -> unit) = f v.x; f v.y; f v.z;
  static member inline Exists (v: ^a Vector3, f: ^a -> bool) = f v.x || f v.y || f v.z
  static member inline ForAll (v: ^a Vector3, f: ^a -> bool) = f v.x && f v.y && f v.z
  static member inline Iter2 (a: ^a Vector3, b: ^b Vector3, f: ^a -> ^b -> unit) = f a.x b.x; f a.y b.y; f a.z b.z;
  static member inline Exists2 (a: ^a Vector3, b: ^b Vector3, f: ^a -> ^b -> bool) = f a.x b.x || f a.y b.y || f a.z b.z
  static member inline ForAll2 (a: ^a Vector3, b: ^b Vector3, f: ^a -> ^b -> bool) = f a.x b.x && f a.y b.y && f a.z b.z

  static member inline Iter' (f: (^a Vector3 -> ^a) -> unit) =
    f (fun v -> v.x)
    f (fun v -> v.y)
    f (fun v -> v.z)
  static member inline Exists' (f: (^a Vector3 -> ^a) -> bool) =
    f (fun v -> v.x) || f (fun v -> v.y) || f (fun v -> v.z)
  static member inline ForAll' (f: (^a Vector3 -> ^a) -> bool) =
    f (fun v -> v.x) && f (fun v -> v.y) && f (fun v -> v.z)

[<Struct>]
type Vector4< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = {
  x : ^a
  y : ^a
  z : ^a
  w : ^a
} with
  static member inline Init(x, y, z, w) = { x = x; y = y; z = z; w = w }
  static member inline Return(k : ^a) = Vector4< ^a >.Init(k, k, k, k)
  static member inline Zero: ^a Vector4 = pure' zero
  static member inline One: ^a Vector4 = pure' one

  static member inline Map(v: ^a Vector4, f: ^a -> ^b)=
    Vector4< ^a >.Init(f v.x, f v.y, f v.z, f v.w)

  static member inline Map2(a, b, f) = {
    x = f a.x b.x
    y = f a.y b.y
    z = f a.z b.z
    w = f a.w b.w
  }

  static member inline (~-) (a: 't Vector4): 't Vector4 = map (~-) a
  static member inline (+) (a, b): 't Vector4 = map2' (+) a b
  static member inline (-) (a, b): 't Vector4 = map2' (-) a b
  static member inline (*) (a, b): 't Vector4 = map2' (*) a b
  static member inline (/) (a, b): 't Vector4 = map2' (/) a b
  static member inline (%) (a, b): 't Vector4 = map2' (%) a b
  static member inline ( *. ) (a: 't, b: 't Vector4) = pure' a * b
  static member inline ( /. ) (a: 't, b: 't Vector4) = pure' a / b
  static member inline ( .* ) (a: 't Vector4, b: 't) = a * pure' b
  static member inline ( ./ ) (a: 't Vector4, b: 't) = a / pure' b
  static member inline ( .% ) (a: 't Vector4, b: 't) = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w

  static member inline ToSeq (v : ^a Vector4) =
    seq { yield v.x; yield v.y; yield v.z; yield v.w }

  static member inline Iter (v: ^a Vector4, f: ^a -> unit) = f v.x; f v.y; f v.z; f v.w
  static member inline Exists (v: ^a Vector4, f: ^a -> bool) = f v.x || f v.y || f v.z || f v.w
  static member inline ForAll (v: ^a Vector4, f: ^a -> bool) = f v.x && f v.y && f v.z && f v.w
  static member inline Iter2 (a: ^a Vector4, b: ^b Vector4, f: ^a -> ^b -> unit) = f a.x b.x; f a.y b.y; f a.z b.z; f a.w b.w
  static member inline Exists2 (a: ^a Vector4, b: ^b Vector4, f: ^a -> ^b -> bool) = f a.x b.x || f a.y b.y || f a.z b.z || f a.w b.w
  static member inline ForAll2 (a: ^a Vector4, b: ^b Vector4, f: ^a -> ^b -> bool) = f a.x b.x && f a.y b.y && f a.z b.z && f a.w b.w

  static member inline Iter' (f: (^a Vector4 -> ^a) -> unit) =
    f (fun v -> v.x)
    f (fun v -> v.y)
    f (fun v -> v.z)
    f (fun v -> v.w)
  static member inline Exists' (f: (^a Vector4 -> ^a) -> bool) =
    f (fun v -> v.x) || f (fun v -> v.y) || f (fun v -> v.z) || f (fun v -> v.w)
  static member inline ForAll' (f: (^a Vector4 -> ^a) -> bool) =
    f (fun v -> v.x) && f (fun v -> v.y) && f (fun v -> v.z) && f (fun v -> v.w)

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector2 =
  let inline init x y : ^a Vector2 = Vector2<_>.Init(x, y)

  let inline angle(v : ^a Vector2) : ^a =
    atan2 v.y v.x

  let inline ofAngle angle =
    init (cos angle) (sin angle)

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector3 =
  let inline init x y z : ^a Vector3 = Vector3<_>.Init(x, y, z)

  let inline cross
    ({ x = x1; y = y1; z = z1 } : 'a Vector3)
    ({ x = x2; y = y2; z = z2 } : 'a Vector3)
    : 'a Vector3 =
    init
      (y1 * z2 - z1 * y2)
      (z1 * x2 - x1 * z2)
      (x1 * y2 - y1 * x2)

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector4 =
  let inline init x y z w : 'a Vector4 = Vector4<_>.Init(x, y, z, w)


type Vector = Vector with
  static member inline VectorImpl(_: 'a Vector2) = Vector
  static member inline VectorImpl(_: 'a Vector3) = Vector
  static member inline VectorImpl(_: 'a Vector4) = Vector

// #nowarn "0064"

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector =
  let inline constraint' v =
    let inline f (_ : ^V) (_ : ^Va ) =
      ((^V or ^Va) : (static member VectorImpl : ^Va -> Vector)
          Unchecked.defaultof<_>
      )

    f Vector v |> ignore

  let inline map (f: 'a -> 'b) (x: ^``Vector<'a>``): ^``Vector<'b>`` =
    if false then constraint' x
    (^``Vector<'a>``: (static member Map: _*_->_) x, f)

  let inline toSeq (x: ^``Vector<'a>``): seq<'a> =
    if false then constraint' x
    (^``Vector<'a>``: (static member ToSeq: _->_) x)

  let inline iter (f: 'a -> unit) (x: ^``Vector<'a>``): unit =
    if false then constraint' x
    (^``Vector<'a>``: (static member Iter: _*_->_) x, f)

  let inline exists (f: 'a -> bool) (x: ^``Vector<'a>``): bool =
    if false then constraint' x
    (^``Vector<'a>``: (static member Exists: _*_->_) x, f)

  let inline forall (f: 'a -> bool) (x: ^``Vector<'a>``): bool =
    if false then constraint' x
    (^``Vector<'a>``: (static member ForAll: _*_->_) x, f)

  let inline iter2 (f: 'a -> 'b -> unit) (x: ^``Vector<'a>``) (y: ^``Vector<'b>``): unit =
    if false then
      constraint' x
      constraint' y
    ((^``Vector<'a>`` or ^``Vector<'b>``): (static member Iter2: _*_*_->_) x,y,f)

  let inline exists2 (f: 'a -> 'b -> bool) (x: ^``Vector<'a>``) (y: ^``Vector<'b>``): bool =
    if false then 
      constraint' x
      constraint' y
    ((^``Vector<'a>`` or ^``Vector<'b>``): (static member Exists2: _*_*_->_) x,y,f)

  let inline forall2 (f: 'a -> 'b -> bool) (x: ^``Vector<'a>``) (y: ^``Vector<'b>``): bool =
    if false then
      constraint' x
      constraint' y
    ((^``Vector<'a>`` or ^``Vector<'b>``): (static member ForAll2: _*_*_->_) x,y,f)

  let inline iter' (f: (^``Vector<'a>`` -> 'a) -> unit): unit =
    if false then constraint' (Unchecked.defaultof< ^``Vector<'a>`` >)
    (^``Vector<'a>``: (static member Iter': _->_) f)

  let inline exists' (f: (^``Vector<'a>`` -> 'a) -> bool): bool =
    if false then constraint' (Unchecked.defaultof< ^``Vector<'a>`` >)
    (^``Vector<'a>``: (static member Exists': _->_) f)

  let inline forall' (f: (^``Vector<'a>`` -> 'a) -> bool): bool =
    if false then constraint' (Unchecked.defaultof< ^``Vector<'a>`` >)
    (^``Vector<'a>``: (static member ForAll': _->_) f)

  let inline dot (a : ^``Vector<'a>``) (b : ^``Vector<'a>``) =
    if false then constraint' a
    (^``Vector<'a>`` : (static member inline Dot :_*_->_) a, b)

  let inline squaredLength (v : '``Vector<'a>``) =
    dot v v

  let inline length (v : '``Vector<'a>``) =
    sqrt (squaredLength v)

  let inline normalize (v : '``Vector<'a>``) : '``Vector<'a>`` =
    let len = length v
    if len = LanguagePrimitives.GenericZero then
      zero
    else
      v ./ len
  
  let inline zero< ^``Vector<'a>``
                when ^``Vector<'a>`` : (static member Zero: ^``Vector<'a>``)
                and (^``Vector<'a>`` or Vector): (static member VectorImpl: ^``Vector<'a>`` -> Vector)
    >=
    if false then constraint'(Unchecked.defaultof< ^``Vector<'a>`` >)
    (zero: ^``Vector<'a>``)

  let inline one< ^``Vector<'a>``
                when ^``Vector<'a>`` : (static member One: ^``Vector<'a>``)
                and (^``Vector<'a>`` or Vector): (static member VectorImpl: ^``Vector<'a>`` -> Vector)
    > =
    if false then constraint'(Unchecked.defaultof< ^``Vector<'a>`` >)
    (one: ^``Vector<'a>``)

  let inline x (v : ^``Vector<'a>``) =
    if false then constraint' v
    (^``Vector<'a>`` : (member x : _) v)

  let inline y (v : ^``Vector<'a>``) =
    if false then constraint' v
    (^``Vector<'a>`` : (member y : _) v)

  let inline z (v : ^``Vector<'a>``) =
    if false then constraint' v
    (^``Vector<'a>`` : (member z : _) v)

  let inline w (v : ^``Vector<'a>``) =
    if false then constraint' v
    (^``Vector<'a>`` : (member w : _) v)

[<AutoOpen>]
module Patterns =
  let inline (|Vector2|) (v: ^a Vector2) = (v.x, v.y)
  let inline (|Vector3|) (v: ^a Vector3) = (v.x, v.y, v.z)
  let inline (|Vector4|) (v: ^a Vector4) = (v.x, v.y, v.z, v.w)
