using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class Ball
    {
        public Vector moment;
        public Vector acc = new Vector(0, 0);
        public Circle frame;
        public Trace trace;
        public double actualTime = 0;

        public Ball(Vector moment, double radius)
        {
            this.moment = moment;
            frame = new Circle(moment.x, moment.y, radius);

            trace = new Trace(this);
        }

        public Ball(Vector moment, Vector acc, double radius) : this(moment, radius)
        {
            this.acc = acc;
        }

        public Ball(double x, double y, double radius) : this(new Vector(x, y, 0, 0), radius)
        {
            
        }

        public Ball(Vector moment, Vector acc, Circle frame, double actualTime)
        {
            this.moment = moment;
            this.acc = acc;
            this.frame = frame;
            this.actualTime = actualTime;

            trace = new Trace(this);
        }

        public void setAcc(Vector acc)
        {
            this.acc = acc;
        }

        public void setActualTime(double time)
        {
            actualTime = time;
        }

        public void incrementActualTime(double addTime)
        {
            actualTime += addTime;
        }

        public void bounce(Wall w)
        {           
            Vector wallV = new Vector(w.l).normalizeDirection();
            double momentAngleNormalized = Vector.normalizeAngle(moment.angle);
            double wallAngleNormalized = Vector.normalizeAngle(wallV.angle);

            double momentAngleMinimized = Vector.minimizeAngle(momentAngleNormalized);
            double wallAngleMinimized = Vector.minimizeAngle(wallAngleNormalized);

            bool sameSide = momentAngleNormalized - wallAngleNormalized <= Math.PI;
            double correctWall = sameSide ? 0 : Math.PI * 2;
            double alpha = Math.Abs(momentAngleMinimized - wallAngleMinimized);
            double betta =
                momentAngleNormalized > wallAngleNormalized + correctWall ? Math.PI * 2 - alpha : alpha;


            bool a = alpha == betta;
            bool b = alpha < Math.PI / 2;

            bool firstCase =
                !a && b ||
                a && !b;

            //bool secondCase =
            //    a && b ||
            //    !a && !b;

            alpha = Math.Min(alpha, Math.Max(Math.PI - alpha, alpha - Math.PI));
            double rotateAngle = (firstCase ? -1 : 1) * 2 * alpha;
            moment.rotate(rotateAngle);
            moment.angle = Vector.normalizeAngle(moment.angle);
        }

        public Ball updateTrace()
        {
            trace = new Trace(this);
            return this;
        }

        public Ball clone()
        {
            return new Ball(moment.clone(), acc.clone(), frame.clone(), actualTime);
        }

        public Ball move(double time)
        {
            Point newPosition = 
                moment.add(acc.dx * time, acc.dy * time)
                .updateEnd(time)
                .getEnd();

            return updateCoords(newPosition);
        }

        public Ball updateCoords(Point p)
        {
            return updateCoords(p.x, p.y);
        }


        public Ball updateCoords(double x, double y)
        {
            moment.setStart(x, y);

            frame.x = x;
            frame.y = y;

            return this;
        }
    }
}
