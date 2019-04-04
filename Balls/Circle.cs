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
        public double M, D, E;

        public Circle(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

            M = -2 * x;
            D = -2 * y;
            E = x * x + y * y - radius * radius;
        }

        public Circle clone()
        {
            return new Circle(x, y, radius);
        }
    }
}
