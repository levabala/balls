﻿namespace BallsModel

open System

exception NotEnoughPointsError of string

[<Struct>]
type Polygone =
  val points: Point array;
  val lines: Line array;
  val normals: Vector array;

  new (points: Point array) =
    if points.Length = 0
    then 
      failwith "Too little points amount"
    
    let closed = Array.append points [|points.[0]|]    

    let pairs = seq {
      for i = 0 to closed.Length - 2 do
        yield (closed.[i], closed.[i + 1])
    }

    let lines : Line array = pairs |> Seq.map Line |> Seq.toArray
    let smallestLine = 
      Array.fold
        (
          fun (acc: Line) (item: Line) -> 
            if abs(item.length) < abs(acc.length) && item.length <> 0M<m>
            then item 
            else acc
        )
        lines.[0]
        lines
    
    if points.Length < 3
    then 
      {
        points = points;
        lines = lines;
        normals = [||]
      }
    else
      let angle = (Vector(lines.[0]), Vector(lines.[1])) ||> Vector.angleBetween
      let clockwise = angle >= 0M<rad>

      if not clockwise
      then Polygone(Array.rev closed)
      else 
        let normals: Vector array = Seq.toArray <| seq {
          for line in lines do
            let center = Point(line.x1 + (line.x2 - line.x1) / 2M, line.y1 + (line.y2 - line.y1) / 2M)
            let leftV = 
              Vector(line)
                .setStart(center.x, center.y)
                .setLength(abs(smallestLine.length))
                .rotate(
                  Math.PI / -2.0
                  |> decimal 
                  |> LanguagePrimitives.DecimalWithMeasure
                )

            let leftVEnd = leftV.endPoint
            let leftXSign = leftVEnd.x - center.x |> sign
            let leftYSign = leftVEnd.y - center.y |> sign
            let pointIsLeft (p: Point) = 
              (p.x - center.x) |> sign = leftXSign &&
              (p.y - center.y) |> sign = leftYSign

            let (leftColls, rightColls) =
              Array.fold
                (fun (leftCollisions, rightCollisions) otherLine ->
                  let interP = Line.intersect line otherLine
                  match interP with
                    | Some p -> 
                        let insideSegment = Line.pointIsOnLineSegment p otherLine
                        match insideSegment with
                          | false -> (leftCollisions, rightCollisions)
                          | true -> 
                            let isLeft = pointIsLeft p
                            let leftInc = if isLeft then 1 else 0
                            let rightInc = abs(leftInc - 1)
              
                            (leftCollisions + leftInc, rightCollisions + rightInc)
                    | None -> (leftCollisions, rightCollisions)
                )
                (0, 0)
                lines

  
            yield leftV
            //let leftDirection = leftColls % 2 <> 0        
        
            //if leftDirection 
            //then yield leftV
            //else yield leftV.rotate(Math.PI |> decimal |> LanguagePrimitives.DecimalWithMeasure)
        }
    
        {
          points = points;
          lines = lines;
          normals = normals;
        }


