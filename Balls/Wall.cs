using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Wall
    {
        public double x1, x2, y1, y2;
        public Line l;
        private Vector normal;
        private double? length;

        public Wall(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;

            l = new Line(x1, y1, x2, y2);
        }

        public Wall(double x1, double y1, double x2, double y2, Line l)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            this.l = l;
        }

        public Wall clone()
        {
            return new Wall(x1, y1, x2, y2, l);
        }

        public double updateWidth()
        {
            double dx = x2 - x1;
            double dy = y2 - y1;

            double actualWidth = Math.Sqrt(dx * dx + dy * dy);
            length = actualWidth;

            return actualWidth;
        }

        public Vector updateNormal()
        {
            normal = new Vector(l).rotate(Math.PI / 2);
            return normal;
        }

        public double Length {
            get
            {
                return length ?? updateWidth();
            }
        }

        public Vector Normal
        {
            get
            {
                return normal ?? updateNormal();
            }
        }
    }
}
