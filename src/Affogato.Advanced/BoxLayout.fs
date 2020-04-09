module Affogato.Advanced.BoxLayout

open Affogato
open Affogato.Helper

[<Interface>]
type IBoxElement =
  abstract Calculate: float32 Rectangle2 -> unit

[<Struct>]
type Direction = Vertical | Horizontal

let inline withDirection dir (f: ('a Vector2 -> _) -> ('a Vector2 -> _) -> _ -> _) =
  dir |> function
  | Vertical   -> f Vector.x Vector.y Vector2.init
  | Horizontal -> f Vector.y Vector.x (flip Vector2.init)


[<Sealed>]
type Column(dir, children: IBoxElement[]) =
  interface IBoxElement with
    member __.Calculate({position=position; size=size}) =
      let count = float32 children.Length

      withDirection dir (fun x y init ->
        let size = init (x size) ((y size) / count)

        for (i, child) in children |> Seq.indexed do
          child.Calculate({
            size = size
            position = init (x position) (y size * float32 i)
          })
      )


[<Sealed>]
type Split(dir, rate, elem1: IBoxElement, elem2: IBoxElement) =
  let rate = rate |> min 1.0f |> max 0.0f

  interface IBoxElement with
    member __.Calculate({position=position; size=size}) =
      withDirection dir (fun x y init ->
        let d = y size * rate

        elem1.Calculate {
          position = position
          size = init (x size) d
        }

        elem2.Calculate {
          position = init (x position) (y position + d)
          size = init (x size) (y size * (1.0f - rate))
        }
      )
