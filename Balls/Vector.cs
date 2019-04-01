using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Vector
    {
        public double x, y, dx, dy, ex, ey, length, angle;

        public Vector(double x, double y, double dx, double dy)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            ex = x + dx;
            ey = y + dy;

            length = Math.Sqrt(dx * dx + dy * dy);

            updateAngle();
        }

        public Vector(double x, double y, double dx, double dy, double length, double angle)
        {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
            this.length = length;
            this.angle = angle;
            ex = x + dx;
            ey = y + dy;
        }

        public Vector(double dx, double dy) : this(0, 0, dx, dy)
        {

        }

        public Vector(Line l): this(l.x1, l.y1, l.x2 - l.x1, l.y2 - l.y1)
        {

        }

        public Line getLine()
        {
            return new Line(this);
        }

        public Vector add(double dx, double dy)
        {
            setCoords(x + dx, y + dy);

            return this;
        }

        public Point getStart()
        {
            return new Point(x, y);
        }

        public Point getEnd()
        {
            return new Point(ex, ey);
        }

        public Vector next(double coeff = 1)
        {
            return setCoords(updateEnd(coeff).getEnd());
        }

        public Vector updateDeltas()
        {
            dy = Math.Sin(angle) * length;
            dx = Math.Cos(angle) * length;

            return this;
        }

        public Vector setLength(double length)
        {
            this.length = length;

            updateDeltas();
            updateEnd();

            return this;
        }

        public Vector updateEnd(double coeff)
        {
            ex = x + dx * coeff;
            ey = y + dy * coeff;

            return this;
        }

        public Vector updateEnd()
        {
            ex = x + dx;
            ey = y + dy;

            return this;
        }

        public Vector updateAngle()
        {
            angle = Math.Atan2(dy, dx);

            return this;
        }

        public Vector setAngle(double angle)
        {
            this.angle = angle;

            updateDeltas();
            updateEnd();

            return this;
        }

        public Vector rotate(double delta)
        {
            return setAngle(angle + delta);
        }

        public Vector setCoords(Point p)
        {
            return this.setCoords(p.x, p.y);
        }

        public Vector setCoords(double x, double y)
        {
            this.y = y;
            this.x = x;

            updateEnd();

            return this;
        }

        public Vector setX(double x, bool doUpdateEnd = true)
        {
            this.x = x;

            if (doUpdateEnd)
                updateEnd();

            return this;
        }

        public Vector setY(double y, bool doUpdateEnd = true)
        {
            this.y = y;

            if (doUpdateEnd)
                updateEnd();

            return this;
        }

        public Vector clone()
        {
            return new Vector(x, y, dx, dy, length, angle);
        }
       

        // normalize direction to I or IV quarter
        public Vector normalizeDirection()
        {
            Quarter currentQuarter;
            double newAngle = normalizeAngle(angle);

            if (newAngle> Math.PI / 2 * 3)
                currentQuarter = Quarter.IV;
            else if (newAngle> Math.PI)
                currentQuarter = Quarter.III;
            else if (newAngle> Math.PI / 2)
                currentQuarter = Quarter.II;
            else
                currentQuarter = Quarter.I;

            if (currentQuarter == Quarter.II || currentQuarter == Quarter.III)
                newAngle += Math.PI;

            setAngle(newAngle);

            return this;
        }

        public static double normalizeAngle(double angle, double eps = 0.00001)
        {
            double sign = Math.Sign(angle);
            angle = Math.Abs(angle);

            double res = angle - Math.Floor((angle + eps) / (Math.PI * 2)) * Math.PI * 2;

            res *= sign;

            if (sign < 0)
                res += Math.PI * 2;

            return res;
        }

        public static double minimizeAngle(double angle, bool normalized = true)
        {
            double res = normalized ? angle : normalizeAngle(angle);
            if (res >= Math.PI)
                res = res - Math.PI * 2;

            return res;
        }
    }

    public enum Quarter
    {
        I,
        II,
        III,
        IV,
    }
}
