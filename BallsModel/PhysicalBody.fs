namespace BallsModel

[<Struct>]
type PhysicalBody =
  val moment: Vector;
  val mass: decimal<kg>;

  new (moment, mass) = 
    {
      moment = moment;
      mass = mass;
    }

  static member hit (ph1: PhysicalBody) (ph2: PhysicalBody) =
    let axis = Line(ph1.moment.startPoint, ph2.mome.endPoint)
    