﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public static class Intersection
    {
        public static bool intersect(Line l1, Line l2, bool likeSegments = false)
        {
            Point p = new Point();
            return intersect(l1, l2, ref p, likeSegments);
        }

        public static bool intersect(Line l1, Line l2, ref Point p, bool likeSegments = false)
        {
            double det = l1.A * l2.B - l1.B * l2.A;

            if (det == 0)
                return false;


            double x = (l2.B * l1.C - l1.B * l2.C) / det;
            double y = (l1.A * l2.C - l2.A * l1.C) / det;

            Point localP = new Point(x, y);
            if (likeSegments)
            {
                double d11 = localP.dist(l1.getStart());
                double d12 = localP.dist(l1.getEnd());
                double length1 = l1.getLength();

                double d21 = localP.dist(l2.getStart());
                double d22 = localP.dist(l2.getEnd());
                double length2 = l2.getLength();

                bool outOf =
                    Math.Max(
                        d11,
                        d12
                    ) > length1 ||
                    Math.Max(
                        d21,
                        d22
                    ) > length2;

                if (outOf)
                    return false;
            }

            p = localP;

            return true;
        }
       
        public static bool intersect(Vector v1, Vector v2)
        {
            Line l1 = new Line(v1);
            Line l2 = new Line(v2);

            Point p = new Point(0, 0);
            return intersect(l1, l2, ref p);
        }

        
        public static Collision intersect(Line l, Circle c, ref Point interP1, ref Point interP2)
        {
            double sign(double value)
            {
                if (value >= 0)
                    return 1;
                return -1;
            }

            // normalize
            l = new Line(l.x1 - c.x, l.y1 - c.y, l.x2 - c.x, l.y2 - c.y);
            c = new Circle(0, 0, c.radius);
            

            double dx = l.x2 - l.x1;
            double dy = l.y2 - l.y1;
            double length = Math.Sqrt(dx * dx + (dy * dy));
            double r = c.radius;

            double D = l.x1 * l.y2 - l.x2 * l.y1;
            double det = Math.Sqrt(r * r * length * length - D * D);

            double leftX = D * dy;
            double rightX = sign(dy) * dx * det;

            double leftY = -D * dx;
            double rightY = Math.Abs(dy) * det;

            double x1 = (leftX + rightX) / (r * r);
            double x2 = (leftX - rightX) / (r * r);

            double y1 = (leftY + rightY) / (r * r);
            double y2 = (leftY - rightY) / (r * r);

            if (det < 0)
                return Collision.NoCollision;

            if (det == 0)
            {
                interP1 = new Point(x1, y1);
                return Collision.PartialCollision;
            }

            interP1 = new Point(x1, y1);
            interP2 = new Point(x2, y2);
            return Collision.FullCollision;
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
