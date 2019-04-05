#load "Measures.fs"
#load "Point.fs"
#load "Line.fs"
#load "Vector.fs"
#load "Polygone.fs"

open BallsModel

let points = [|
  Point(1M<m>, 1M<m>); 
  Point(3M<m>, 2M<m>); 
  Point(2M<m>, 5M<m>); 
  Point(-2M<m>, 7M<m>)
|]

let poly = Polygone(points)
