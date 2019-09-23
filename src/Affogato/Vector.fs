namespace Affogato

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
    Vector2< ^a >.Init(f v.x, f v.y)

  static member inline Map2(a, b, f) = {
    x = f a.x b.x
    y = f a.y b.y
  }

  static member inline (~-) (a): 'a Vector2 = map (~-) a
  static member inline (+) (a, b): 'a Vector2 = map2 (+) a b
  static member inline (-) (a, b): 'a Vector2 = map2 (-) a b
  static member inline (*) (a, b): 'a Vector2 = map2 (*) a b
  static member inline (/) (a, b): 'a Vector2 = map2 (/) a b
  static member inline (%) (a, b): 'a Vector2 = map2 (%) a b
  static member inline ( .* ) (a, b): 'a Vector2 = a * pure' b
  static member inline ( *. ) (a, b): 'a Vector2 = pure' a * b
  static member inline ( ./ ) (a, b): 'a Vector2 = a / pure' b
  static member inline ( /. ) (a, b): 'a Vector2 = pure' a / b
  static member inline ( .% ) (a, b): 'a Vector2 = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y

  static member inline ToSeq (v : ^a Vector2) =
    seq { yield v.x; yield v.y }

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

  static member inline (~-) (a): 'a Vector3 = map (~-) a
  static member inline (+) (a, b): 'a Vector3 = map2 (+) a b
  static member inline (-) (a, b): 'a Vector3 = map2 (-) a b
  static member inline (*) (a, b): 'a Vector3 = map2 (*) a b
  static member inline (/) (a, b): 'a Vector3 = map2 (/) a b
  static member inline (%) (a, b): 'a Vector3 = map2 (%) a b
  static member inline ( .* ) (a, b): 'a Vector3 = a * pure' b
  static member inline ( *. ) (a, b): 'a Vector3 = pure' a * b
  static member inline ( ./ ) (a, b): 'a Vector3 = a / pure' b
  static member inline ( /. ) (a, b): 'a Vector3 = pure' a / b
  static member inline ( .% ) (a, b): 'a Vector3 = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y + a.z * b.z

  static member inline ToSeq (v : ^a Vector3) =
    seq { yield v.x; yield v.y; yield v.z }

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

  static member inline (~-) (a): 'a Vector4 = map (~-) a
  static member inline (+) (a, b): 'a Vector4 = map2 (+) a b
  static member inline (-) (a, b): 'a Vector4 = map2 (-) a b
  static member inline (*) (a, b): 'a Vector4 = map2 (*) a b
  static member inline (/) (a, b): 'a Vector4 = map2 (/) a b
  static member inline (%) (a, b): 'a Vector4 = map2 (%) a b
  static member inline ( .* ) (a, b): 'a Vector4 = a * pure' b
  static member inline ( *. ) (a, b): 'a Vector4 = pure' a * b
  static member inline ( ./ ) (a, b): 'a Vector4 = a / pure' b
  static member inline ( /. ) (a, b): 'a Vector4 = pure' a / b
  static member inline ( .% ) (a, b): 'a Vector4 = a % pure' b

  static member inline Dot(a, b): 'a = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w

  static member inline ToSeq (v : ^a Vector4) =
    seq { yield v.x; yield v.y; yield v.z; yield v.w }


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

#nowarn "0064"

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector =
  let inline constraint' v =
    let inline f (_ : ^V) (_ : ^Va ) =
      ((^V or ^Va) : (static member VectorImpl : ^Va -> Vector)
          Unchecked.defaultof<_>
      )

    f Vector v |> ignore

  let inline dot (a : ^``Vector<'a>``) (b : ^``Vector<'a>``) =
    constraint' a
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
  
  let inline zero(): ^``Vector<'a>`` =
    constraint'(Unchecked.defaultof< ^``Vector<'a>`` >)
    zero

  let inline one(): ^``Vector<'a>`` =
    constraint'(Unchecked.defaultof< ^``Vector<'a>`` >)
    one

  let inline x (v : ^``Vector<'a>``) =
    constraint' v
    (^``Vector<'a>`` : (member x : _) v)

  let inline y (v : ^``Vector<'a>``) =
    constraint' v
    (^``Vector<'a>`` : (member y : _) v)

  let inline z (v : ^``Vector<'a>``) =
    constraint' v
    (^``Vector<'a>`` : (member z : _) v)

  let inline w (v : ^``Vector<'a>``) =
    constraint' v
    (^``Vector<'a>`` : (member w : _) v)
