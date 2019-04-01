using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Point
    {
        public double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point() : this(0,0)
        {

        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return Equals(obj as Point);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public bool Equals(Point p)
        {
            return p.x == x && p.y == y;

        }

        public double dist(Point p)
        {
            double dx = p.x - x;
            double dy = p.y - y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
