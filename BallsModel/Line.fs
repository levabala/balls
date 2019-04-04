namespace BallsModel

[<Struct>]
type Line =
  val x1: decimal<m>;
  val y1: decimal<m>;
  val x2: decimal<m>;
  val y2: decimal<m>;
  val A: decimal<m>;
  val B: decimal<m>;
  val C: decimal<m^2>;

  new (x1, y1, x2, y2) = 
    let A = y2 - y1
    let B = x1 - x2
    let C = A * x1 + B * y1

    {
      x1 = x1;
      y1 = y1;
      x2 = x2;
      y2 = y2;
      A = A;
      B = B;
      C = C;
    }

  new (p1: Point, p2: Point) = 
    Line(p1.x, p1.y, p2.x, p2.y)
  
  static member angleBetween (l1: Line) (l2: Line) : decimal<rad> = 
    let A1 = l1.A |> decimal |> float
    let B1 = l1.B |> decimal |> float
    let A2 = l2.A |> decimal |> float
    let B2 = l2.B |> decimal |> float

    let top = (A1 * A2 + B1 * B2)
    let bottom = 
      ((A1 ** 2.0 + B1 ** 2.0) |> sqrt) *
      ((A2 ** 2.0 + B2 ** 2.0) |> sqrt)

    top / bottom |> acos |> decimal |> LanguagePrimitives.DecimalWithMeasure
