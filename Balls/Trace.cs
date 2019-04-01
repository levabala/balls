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

        public bool checkCollision(Line line, Point origin, ref Point interP)
        {
            //List<Point> interPoints = new List<Point>();
            //bool intersected = lines.Select(l =>
            //{
            //    Point localInterP = new Point();
            //    bool intersection = Intersection.intersect(line, l, ref localInterP, true);

            //    if (intersection)
            //        interPoints.Add(localInterP);

            //    return intersection;
            //}).Any(l => l);

            Point[] localInterPoints = new Point[]
            {
                new Point(),
                new Point(),
                new Point(),
                new Point(),
            };


            bool intersected1 = Intersection.intersect(line, leftLine, ref localInterPoints[0], true);
            bool intersected2 = Intersection.intersect(line, rightLine, ref localInterPoints[1], true);
            bool intersected3 = Intersection.intersect(line, rootLine, ref localInterPoints[2], true);
            bool intersected4 = Intersection.intersect(line, finalLine, ref localInterPoints[3], true);

            if (!(intersected1 || intersected2 || intersected3 || intersected4))
                return false;

            interP = localInterPoints.Aggregate((acc, val) => origin.dist(val) < origin.dist(acc) ? val : acc);
            return true;
        }
    }
}
