namespace Affogato

open Affogato.Helper

[<Struct>]
type Line< ^v
    when ^v: (static member Zero: ^v)
    and  ^v: (static member One: ^v)
    and  ^v: struct
  > = {
  startPoint : ^v
  endPoint : ^v
} with
  static member inline Init(startPoint, endPoint) = {
    startPoint = startPoint
    endPoint = endPoint
  }
  static member inline Return k = Line< ^v >.Init(k, k)
  static member inline Zero: Line< ^v > = pure' zero
  static member inline One: Line< ^v > = pure' one
  static member inline Map(x, f) = {
    startPoint = f x.startPoint
    endPoint = f x.endPoint
  }


type Line2< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = ^a Vector2 Line

type Line3< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = ^a Vector3 Line

type Line4< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = ^a Vector4 Line


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Line =
  let inline init s e = Line<_>.Init(s, e)
  let inline startPoint l = l.startPoint
  let inline endPoint l = l.endPoint

  let inline equal a b =
    (a.startPoint = b.startPoint && a.endPoint = b.endPoint)
    || (a.endPoint = b.startPoint && a.startPoint = b.endPoint)

  let inline length (l : '``Vector<'a>`` Line) =
    Vector.length(l.startPoint - l.endPoint)

  let inline squaredLength (l : '``Vector<'a>`` Line) =
    Vector.squaredLength(l.startPoint - l.endPoint)


type Rectangle< ^v
      when ^v: (static member Zero: ^v)
      and  ^v: (static member One: ^v)
      and  ^v: struct
  > = {
    position : ^v
    size : ^v
} with
  static member inline Init(position, size) = {
    position = position
    size = size
  }
  static member inline Return k = Rectangle< ^v >.Init(k,k)
  static member inline Zero: ^v Rectangle = pure' zero
  static member inline One: ^v Rectangle = pure' one
  static member inline Map(x, f) = {
    position = f x.position
    size = f x.size
  }


type Rectangle2< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector2 Rectangle

type Rectangle3< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector3 Rectangle

type Rectangle4< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector4 Rectangle


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Rectangle =
  let inline init position size = Rectangle<_>.Init(position, size)
  let inline position r = r.position
  let inline size r = r.size

  let inline left r = r.position.x
  let inline right r = r.position.x + r.size.x
  let inline up r = r.position.y
  let inline down r = r.position.y + r.size.y

  let inline diagonalPosition (r: '``Vector<'a>`` Rectangle) : '``Vector<'a>`` =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)
    r.position + r.size

  let inline centerPosition (r: '``Vector<'a>`` Rectangle) : '``Vector<'a>`` =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)
    let two: '``Vector<'a>`` = one + one
    r.position + r.size / two

  let inline lurd r : ('``Vector<'a>`` * '``Vector<'a>``) =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)
    r.position, diagonalPosition r

  let inline isCollidedAxis(axis : '``Vector<'a>`` -> 'a) (aLU, aRD) (bLU, bRD) : bool =
    Utils.inCollision (axis aLU, axis aRD) (axis bLU, axis bRD)

  let inline isInside (p : '``Vector<'a>``) r : bool =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)

    let lu, rd = lurd r
    
    Seq.zip (toSeq lu) (toSeq rd)
    |> Seq.zip (toSeq p)
    |> Seq.forall(fun (p', (lu', rd')) ->
      lu' <= p' && p' <= rd')


  let inline isCollided2 (a : ^a Rectangle2) (b : ^a Rectangle2) : bool =
    let aLU, aRD = lurd a
    let bLU, bRD = lurd b

    let inline f (axis : _ -> ^a) =
      not(axis bRD < axis aLU || axis aRD < axis bLU)

    (f Vector.x) && (f Vector.y)

[<Struct>]
type Sphere< ^a, ^v
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
    and  ^v: (static member Zero: ^v)
    and  ^v: (static member One: ^v)
    and  ^v: struct
  > = {
    radius : ^a
    center : ^v
} with

  static member inline Init(center, radius) = {
    radius = radius
    center = center
  }
  static member inline Return (k: ^t) = {
    radius = k
    center = pure' k
  }
  static member inline Zero: Sphere< ^a, ^v > = Sphere< ^a, ^v >.Init(zero, zero)
  static member inline One: Sphere< ^a, ^v > = Sphere< ^a, ^v >.Init(one, one)
  static member inline Map(x, f) = {
    radius = f x.radius
    center = map f x.center
  }

type Sphere2< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = Sphere< ^a, Vector2< ^a > >

type Sphere3< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = Sphere< ^a, Vector3< ^a > >

type Sphere4< ^a
    when ^a: (static member Zero: ^a)
    and  ^a: (static member One: ^a)
    and  ^a: struct
  > = Sphere< ^a, Vector4< ^a > >

[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Sphere =
  let inline init (c : '``Vector<'a>``) (r : 'a) = Sphere< 'a, '``Vector<'a>`` >.Init(c, r)

  let inline center c = c.center
  let inline radius c = c.radius

  let inline isInside (p : 'a) (c : Sphere<'a, '``Vector<'a>``>) : bool =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)

    let distance = Vector.squaredLength(p - c.center)
    distance < (c.radius * c.radius)

  let inline isCollided (a : Sphere<'a, '``Vector<'a>``>) (b : Sphere<'a, '``Vector<'a>``>) : bool =
    Vector.constraint' (Unchecked.defaultof< '``Vector<'a>`` >)

    let distance = (a.center - b.center) |> Vector.squaredLength
    let radiusSum =
      let x = (a.radius + b.radius)
      x * x

    distance <= radiusSum

[<Struct>]
type Triangle< ^v
    when ^v: (static member Zero: ^v)
    and  ^v: (static member One: ^v)
    and  ^v: struct
  > = {
    p1 : ^v
    p2 : ^v
    p3 : ^v
} with
  static member inline Init(p1, p2, p3) = {
    p1 = p1
    p2 = p2
    p3 = p3
  }

  static member inline Return k = Triangle< ^v >.Init(k,k,k)
  static member inline Zero: ^v Triangle = pure' zero
  static member inline One: ^v Triangle = pure' one
  static member inline Map(x, f) = {
    p1 = f x.p1
    p2 = f x.p2
    p3 = f x.p3
  }


type Triangle2< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector2 Triangle

type Triangle3< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector3 Triangle

type Triangle4< ^a
      when ^a: (static member Zero: ^a)
      and  ^a: (static member One: ^a)
      and  ^a: struct
  > = ^a Vector4 Triangle


[<RequireQualifiedAccess; CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Triangle =
  let inline init p1 p2 p3 = Triangle<_>.Init(p1, p2, p3)

  let inline p1 t = t.p1
  let inline p2 t = t.p2
  let inline p3 t = t.p3

  let inline equal a b =
    let inline f q1 q2 q3 =
      (a.p1 = q1 b) && (a.p2 = q2 b) && (a.p3 = q3 b)

    (f p1 p2 p3) || (f p2 p3 p1) || (f p3 p1 p2) ||
    (f p1 p3 p2) || (f p2 p1 p3) || (f p3 p2 p1)

  let inline hasCommonPoint a b =
    seq {
      let l = [p1; p2; p3]
      for s in l do
      for t in l do
        yield (s, t)
    }
    |> Seq.exists(fun (s, t) -> s a = t b)

  let circumscribedCircle (t : float32 Triangle2) : float32 Sphere2 =
    let x1, y1 = t.p1.x, t.p1.y
    let x2, y2 = t.p2.x, t.p2.y
    let x3, y3 = t.p3.x, t.p3.y
    let c = 2.0f * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1))

    let x =
      ( (y3 - y1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1)
      + (y1 - y2) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / c

    let y =
      ( (x1 - x3) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1)
      + (x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1)) / c

    let center = Vector2.init x y

    Vector2.init (center.x - x1) (center.y - y1)
    |> Vector.length
    |> Sphere.init center