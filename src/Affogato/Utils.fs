namespace Affogato

module Angle =
  let [<Literal>] PI = 3.14159265f
  let inline degreeToRadian degree =
    degree * PI / 180.0f

  let inline radianToDegree radian =
    radian * 180.0f / PI


module Utils =
  let inline inCollision (aLeft : 'a, aRight : 'a) (bLeft, bRight) =
    not (aRight < bLeft || bRight < aLeft)

  let inline boxMullersMethod u1 u2 =
    let a = sqrt(-2.0f * log u1)
    let x = 2.0f * Angle.PI * u2
    a * cos x, a * sin x


module BinarySearch =
  let inline binarySearch count predicate (current : 'a) (target : 'a) : 'a =
    let two : 'a = one + one

    let rec search count diffSum current target =
      if count <= 0 then diffSum
      else
        let middle = (current + target) / two
        let newDiffSum = diffSum + (middle - current)

        if predicate newDiffSum then
          search (count - 1) newDiffSum middle target
        else
          search (count - 1) diffSum current middle

    search count (zero : 'a) current target