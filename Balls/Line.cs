using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Line
    {
        public readonly double A, B, C;
        public readonly double x1, x2, y1, y2;

        public Line(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;

            A = y2 - y1;
            B = x1 - x2;
            C = A * x1 + B * y1;
        }

        public Line(Point p1, Point p2) : this(p1.x, p1.y, p2.x, p2.y)
        {

        }

        public Line(Vector v): this(v.x, v.y, v.ex, v.ey)
        {

        }

        public Line(double A, double B, double C, double x1, double y1, double x2, double y2)
        {
            this.A = A;
            this.B = B;
            this.C = C;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public Line clone()
        {
            return new Line(A, B, C, x1, y1, x2, y2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Line)
                return Equals(obj as Line);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    
        public bool Equals(Line l)
        {
            return l.x1 == x1 && l.y1 == y1 && l.x2 == x2 && l.y2 == y2;
        }
    }
}
