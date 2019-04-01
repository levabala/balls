using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Rect
    {
        private Point p1, p2, p3, p4;
        public Line l1, l2, l3, l4;

        public Rect(Point p1, Point p2, Point p3, Point p4)
        {
            l1 = new Line(p1, p2);
            l2 = new Line(p2, p3);
            l3 = new Line(p3, p4);
            l4 = new Line(p4, p1);

            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.p4 = p4;
        }

        public Rect(Line l1, Line l2, Line l3, Line l4)
        {
            this.l1 = l1;
            this.l2 = l2;
            this.l3 = l3;
            this.l4 = l4;

            p1 = new Point(l1.x1, l1.y1);
            p2 = new Point(l2.x1, l2.y1);
            p3 = new Point(l3.x1, l3.y1);
            p4 = new Point(l4.x1, l4.y1);
        }

        public override bool Equals(object obj)
        {
            if (obj is Rect)
                return Equals(obj as Rect);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(Rect r)
        {
            return r.p1.Equals(p1) && r.p2.Equals(p2) && r.p3.Equals(p3 )&& r.p4.Equals(p4);
        }

        public Rect processWith(Func<Point, Point> processor)
        {
            return new Rect(processor(p1), processor(p2), processor(p3), processor(p4));
        }
    }
}
