using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Circle
    {
        public double x, y, radius;

        public Circle(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
        }

        public Circle clone()
        {
            return new Circle(x, y, radius);
        }

        public Rect getMoveTrace(Vector v)
        {
            v = v.clone();
            v.setCoords(x, y);

            Point startSideP1 = 
                v.clone()
                .setLength(radius)
                .rotate(Math.PI / 2)
                .updateEnd()
                .getEnd();

            Point startSideP2 =
                v.clone()
                .setLength(radius)
                .rotate(-1 * Math.PI / 2)
                .updateEnd()
                .getEnd();


            v.next();

            Point endSideP1 =
                v.clone()
                .setLength(radius)
                .rotate(Math.PI / 2)
                .updateEnd()
                .getEnd();

            Point endSideP2 =
                v.clone()
                .setLength(radius)
                .rotate(-1 * Math.PI / 2)
                .updateEnd()
                .getEnd();

            return new Rect(startSideP1, startSideP2, endSideP1, endSideP2);
        }
    }
}
