namespace BallsModel

[<Struct>]
type PhysicalBody =
  val v: Vector;
  val mass: decimal<kg>;

  new (moment, mass) = 
    {
      v = moment;
      mass = mass;
    }

  member this.setVector (v: Vector) =
    PhysicalBody(v, this.mass)

  static member bounce (ph1: PhysicalBody, ph2: PhysicalBody) : PhysicalBody*PhysicalBody = 
    let axis = Line(ph1.v.startPoint, ph2.v.startPoint)
    let alpha = Vector(axis).angle

    let v1 = ph1.v.rotate(alpha)
    let v2 = ph2.v.rotate(alpha)

    let m1 = ph1.mass
    let m2 = ph2.mass

    let dx1 = (2M * (m2 * v2.dx) + (m1 - m2) * v1.dx) / (m1 + m2)
    let dx2 = (2M * (m1 * v1.dx) + (m2 - m1) * v2.dx) / (m1 + m2)

    let v1' = Vector(v1.x, v1.y, dx1, v1.dy)
    let v2' = Vector(v2.x, v2.y, dx2, v2.dy)

    let v1'' = v1.rotate(-alpha)
    let v2'' = v2.rotate(-alpha)

    (ph1.setVector(v1''), ph2.setVector(v2''))

    