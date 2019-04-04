namespace BallsModel

[<Struct>]
type Ball =
  val moment: Vector;
  val frame: Circle;

  new (moment, frame) = 
    {
      moment = moment;
      frame = frame;
    }