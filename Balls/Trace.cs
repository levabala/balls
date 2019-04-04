using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Trace
    {
        public const double viewDistance = 10000;

        public Vector directionVector, perpVector;
        public Line rootLine, leftLine, rightLine, finalLine;
        public double width;

        public Line[] lines;

        public Trace(Vector directionVector, Line rootLine, Line leftLine, Line rightLine, double width)
        {
            this.directionVector = directionVector;
            this.rootLine = rootLine;
            this.leftLine = leftLine;
            this.rightLine = rightLine;
            this.width = width;            

            updatePerpVector();
            updateFinalLine();
        }

        public Trace(Ball ball)
        {
            rebuild(ball.moment.clone(), ball.frame.radius * 2);
        }

        public void updatePerpVector()
        {
            perpVector = directionVector.clone().rotate(Math.PI / 2).setLength(width / 2);
        }

        public void rebuild(Vector directionVector, double width)
        {
            this.width = width;
            this.directionVector = directionVector.setLength(viewDistance);

            updatePerpVector();

            Point p1 = perpVector.getEnd();
            Point p2 = perpVector.rotate(Math.PI).getEnd();
            Point p12 = directionVector.setStart(p1).getEnd();
            Point p22 = directionVector.setStart(p2).getEnd();

            rootLine = new Line(p1, p2);
            leftLine = new Line(p1, p12);
            rightLine = new Line(p2, p22);

            updateFinalLine();
        }

        public void updateFinalLine()
        {
            Vector finalPerp = perpVector.clone().setStart(directionVector.getEnd());
            Point p1 = perpVector.getEnd();
            Point p2 = perpVector.rotate(Math.PI).getEnd();         

            finalLine = new Line(p1, p2);

            lines = new Line[]
            {
                rootLine,
                leftLine,
                rightLine,
                finalLine,
            };
        }

        public void updateLength(double length)
        {
            directionVector.setLength(length);
            updatePerpVector();
            updateFinalLine();
        }

        public Collision checkCollision(Line line, Point origin, ref Point interP)
        {
            Point rightInterPoint = new Point();
            Point leftInterPoint = new Point();
            bool intersectedRight = Intersection.intersect(line, rightLine, ref rightInterPoint, true);
            bool intersectedLeft = Intersection.intersect(line, leftLine, ref leftInterPoint, true);

            if (intersectedRight && intersectedLeft)
            {
                Point centerInterPoint = new Point();
                bool intersected3 = Intersection.intersect(
                    line, 
                    directionVector.getLine(), 
                    ref centerInterPoint, 
                    true
                );

                interP = centerInterPoint;

                return Collision.FullCollision;
            }

            if (!intersectedRight && !intersectedLeft)
                return Collision.NoCollision;

            if (intersectedRight)
                interP = rightInterPoint;
            else
                interP = leftInterPoint;

            return Collision.PartialCollision;
        }
    }

    public enum Collision {
        NoCollision,
        FullCollision,
        PartialCollision,
    }
}
