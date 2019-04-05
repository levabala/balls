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
  val startP: Point;
  val endP: Point;
  val length: decimal<m>

  new (x1, y1, x2, y2) = 
    let A = y2 - y1
    let B = x1 - x2
    let C = A * x1 + B * y1
    let startP = Point(x1, y1);
    let endP = Point(x2, y2);

    {
      x1 = x1;
      y1 = y1;
      x2 = x2;
      y2 = y2;
      A = A;
      B = B;
      C = C;
      startP = startP;
      endP = endP;
      length = Point.dist startP endP
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

  static member intersect (l1: Line) (l2: Line) : Point option = 
    let det = l1.A * l2.B - l1.B * l2.A
    
    if det = 0M<m^2>
    then None
    else
      let x = (l2.B * l1.C - l1.B * l2.C) / det
      let y = (l1.A * l2.C - l2.A * l1.C) / det

      Point(x, y) |> Some

  static member pointIsOnLine (p: Point) (l: Line) : bool = 
    let eps : decimal<m^2> = 10.0 ** -6.0 |> decimal |> LanguagePrimitives.DecimalWithMeasure

    abs(l.A * p.x + l.B * p.y - l.C) < eps

  static member pointIsOnLineSegment (p: Point) (l: Line) : bool =
    let isOnLine = Line.pointIsOnLine p l

    if not isOnLine
    then false
    else
      let distance = max (Point.dist p l.startP) (Point.dist p l.endP)

      distance <= l.length

  static member countCollisions (l: Line) (lines: Line array) =
    Array.fold 
      (fun acc item -> if Line.intersect l item <> None then acc + 1 else acc)
      0
      lines