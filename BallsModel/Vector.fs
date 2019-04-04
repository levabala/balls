namespace BallsModel

module private Other = 
  let atan2M (dy: decimal<m>) (dx: decimal<m>) : decimal<rad> = 
      let dxF = dx |> decimal |> float
      let dyF = dy |> decimal |> float

      atan2 dyF dxF |> decimal |> LanguagePrimitives.DecimalWithMeasure

type Vector = 
  struct
    val x: decimal<m>; 
    val y: decimal<m>;
    val ex: decimal<m>; 
    val ey: decimal<m>;
    val dx: decimal<m>; 
    val dy: decimal<m>;
    val length: decimal<m>;
    val angle: decimal<rad>;

    new (x: decimal<m>, y: decimal<m>, dx: decimal<m>, dy: decimal<m>) = 
      {
        x = x; 
        y = y; 
        ex = x + dx;
        ey = y + dy;
        dx = dx;
        dy = dy;
        length = 
          dx * dx + dy * dy 
          |> decimal |> float |> sqrt |> decimal 
          |> LanguagePrimitives.DecimalWithMeasure;

        angle = Other.atan2M dy dx;
      }

    new (x: decimal<m>, y: decimal<m>, ex: decimal<m>, ey: decimal<m>, magicSignature) =     
      let dx = ex - x;
      let dy = ey - y;

      {
        x = x; 
        y = y; 
        ex = ex;
        ey = ey;
        dx = dx;
        dy = dy;
        length = 
          dx * dx + dy * dy 
          |> decimal |> float |> sqrt |> decimal 
          |> LanguagePrimitives.DecimalWithMeasure;
        angle = Other.atan2M dy dx;
      }

    new (l: Line) = Vector(l.x1, l.y1, l.x2, l.y2, obj)
    
    new (
        x: decimal<m>,        
        y: decimal<m>, 
        dx: decimal<m>,
        dy: decimal<m>,
        ex: decimal<m>,
        ey: decimal<m>,
        length: decimal<m>, 
        angle: decimal<rad>
      )
     =           
      {
        x = x; 
        y = y; 
        ex = ex;
        ey = ey;
        dx = dx;
        dy = dy;
        length = length;
        angle = angle;
      }

    new (
        x: decimal<m>, 
        y: decimal<m>, 
        length: decimal<m>, 
        angle: decimal<rad>, 
        magicSignature, 
        magicSignature2
      ) =     
      let dx = (x |> decimal |> float |> cos |> decimal) * length
      let dy = (y |> decimal |> float |> sin |> decimal) * length

      {
        x = x; 
        y = y; 
        ex = x + dx;
        ey = y + dy;
        dx = dx;
        dy = dy;
        length = length;
        angle = angle;
      }

    member this.clone : Vector = 
      Vector(this.x, this.y, this.dx, this.dy, this.ex, this.ey, this.length, this.angle)

    member this.rotate (angle: decimal<rad>): Vector=
      Vector(this.x, this.y, this.length, this.angle + angle, obj, obj)

    static member angleBetween (v1: Vector) (v2: Vector) : decimal<rad> = 
      let delta1 = v2.angle - v1.angle
      let delta2 = v2.angle - v1.angle

      if abs(delta1) < abs(delta2)
      then delta1
      else delta2

    static member angleBetweenPositive (v1: Vector) (v2: Vector) : decimal<rad> = 
      if v1.angle > v2.angle 
      then v1.angle - v2.angle 
      else v2.angle - v1.angle
    
  end


