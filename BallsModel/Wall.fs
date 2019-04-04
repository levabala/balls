namespace BallsModel

[<Struct>]
type Wall = 
  val planes: Line*Vector array;

  new (points: Point array) = 
    let buildNormal (line: Line) : Vector =
        Vector(line).rotate(Math.PI)

    let correctNormals (normalPrimal: Vector, normalSecondary: Vector) =


    let closed = points @ [points[0]]
    let pairs: seq Point*Point = seq {
      for i = 0 to closed.Length - 1 do
        yield (closed[i], closed[i + 1])
    }

    let normals = pairs |> Seq.map Line |> Seq.map buildNormal



