using System;
using System.Collections.Generic;
using System.Drawing;
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

        public bool atLine(Line l, double eps = 0.0001)
        {
            return Math.Abs(l.A * x + l.B * y - l.C) < eps;
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

        public bool isBetween(Line l1, Line l2)
        {
            Line perpLine = new Line(this, l1.getStart());

            Point interP1 = l1.getStart();
            Point interP2 = new Point();

            bool intersected = Intersection.intersect(l2, perpLine, ref interP2);
            if (!intersected)
                return false;

            double dx1 = x - interP1.x;
            double dx2 = x - interP2.x;
            double dy1 = y - interP1.y;
            double dy2 = y - interP2.y;
            bool between =
                Math.Sign((dx1 == 0 ? 1 : dx1) * (dy1 == 0 ? 1 : dy1)) !=
                Math.Sign((dx2 == 0 ? 1 : dx2) * (dy2 == 0 ? 1 : dy2));

            return between;
        }

        public void render(Graphics g, Brush brush, float radius)
        {
            g.FillEllipse(brush, (float)x - radius / 2, (float)y - radius / 2, radius, radius);
        }
    }
}
