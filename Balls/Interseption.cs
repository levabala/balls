using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public static class Intersection
    {
        public static bool intersect(Line l1, Line l2, ref Point p)
        {
            double det = l1.A * l2.B - l1.B * l2.A;

            if (det == 0)
                return false;


            double x = (l2.B * l1.C - l1.B * l2.C) / det;
            double y = (l1.A * l2.C - l2.A * l1.C) / det;

            p = new Point(x, y);

            return true;
        }

        public static bool intersect(Vector v1, Vector v2)
        {
            Line l1 = new Line(v1);
            Line l2 = new Line(v2);

            Point p = new Point(0, 0);
            return intersect(l1, l2, ref p);
        }

        
        public static bool intersect(Line l, Circle c, ref Point interP)
        {
            Vector lineVector = new Vector(l);

            Vector perp = lineVector
                .clone()
                .setCoords(c.x, c.y)
                .rotate(Math.PI / 2);

            Line l1 = new Line(lineVector);
            Line l2 = new Line(perp);
            
            intersect(l1, l2, ref interP);

            double dx = interP.x - c.x;
            double dy = interP.y - c.y;

            double dist = Math.Sqrt(dx * dx + dy * dy);
            return dist <= c.radius;
        }

        public static bool intersect(Point p, Wall w, bool pointOnLine = false, double eps = 0.00001)
        {
            pointOnLine = pointOnLine || Math.Abs(w.l.A * p.x + w.l.B * p.y - w.l.C) < eps;

            if (!pointOnLine)
                return pointOnLine;

            double dx1 = p.x - w.l.x1;
            double dy1 = p.y - w.l.y1;

            double dx2 = p.x - w.l.x2;
            double dy2 = p.y - w.l.y2;

            double dist = Math.Max(Math.Sqrt(dx1 * dx1 + dy1 * dy1), Math.Sqrt(dx2 * dx2 + dy2 * dy2));
            return dist < w.Length;
        }
    }
}
