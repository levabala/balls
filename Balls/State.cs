using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    public class State
    {
        public double startTime;
        public List<Ball> balls;
        public List<Wall> walls;

        public State(List<Ball> balls, List<Wall> walls, double time, bool isNew = false)
        {
            this.balls = balls;
            this.walls = walls;
            startTime = time;

            if (isNew)
                balls.ForEach(ball => ball.setActualTime(startTime));
        }

        public State calcNextState()
        {
            List<Ball> newBalls = balls.Select(b => b.clone()).ToList();
            List<Wall> newWalls = walls.Select(w => w.clone()).ToList();

            // I. Find the closest intersection time for each ball
            IEnumerable<Tuple<double, Ball, Wall>> closestIntersections = newBalls.Select(ball =>
            {
            Tuple<Point, Wall>[] intersections = newWalls.Select(wall =>
            {
                Point interP = new Point();
                bool intersected = Intersection.intersect(wall.l, ball.moment.getLine(), ref interP);

                intersected = intersected && Intersection.intersect(interP, wall, true);

                if (intersected)
                {
                    double pdx = Math.Round(interP.x - ball.moment.x, 5);
                    double pdy = Math.Round(interP.y - ball.moment.y, 5);

                    bool rightSide =
                        Math.Sign(pdx) == Math.Sign(Math.Round(ball.moment.dx, 5)) &&
                        Math.Sign(pdy) == Math.Sign(Math.Round(ball.moment.dy, 5));                   

                    if (rightSide)
                        return new Tuple<Point, Wall>(interP, wall);
                    else return null;
                }
                return null;
            }).Where(p => p != null).ToArray();

            if (intersections.Length == 0)
                return null;

            IEnumerable<Tuple<double, Tuple<Point, Wall>>> withDistances = intersections.Select(
                t => new Tuple<double, Tuple<Point, Wall>>(
                    ball.moment.getStart().dist(t.Item1), t
                    )
                );

            Tuple<double, Tuple<Point, Wall>> minIntersection = withDistances.Aggregate((acc, val) =>
                val.Item1 < acc.Item1 ? val : acc
            );


            Wall interWall = minIntersection.Item2.Item2;
            Line perpLine = ball.moment.clone().setAngle(new Vector(interWall.l).rotate(Math.PI / 2).angle).getLine();

            Point perpInterP = new Point();
            Intersection.intersect(interWall.l, perpLine, ref perpInterP);

            double length = (ball.frame.radius * minIntersection.Item1) / ball.moment.getStart().dist(perpInterP);
            //double length = 0;

            double distToBounce = minIntersection.Item1 - length;
            double timeLeftToIntersect = distToBounce / ball.moment.length;
            return new Tuple<double, Ball, Wall>(timeLeftToIntersect, ball, minIntersection.Item2.Item2);
            }).Where(p => p != null).ToArray(); ;

            if (closestIntersections.Count() == 0)
                return this;

            // II. Find the closest intersection at all
            Tuple<double, Ball, Wall> closestIntersection = closestIntersections.Aggregate((acc, val) =>
                val.Item1 < acc.Item1 ? val : acc
            );

            //double minMove = closestIntersection.Item2.moment.length / 1000;
            //IEnumerable<Tuple<double, Ball, Wall>> intersectionsToProcess = closestIntersections.Where((inter) =>
            //    minMove > inter.Item1
            //);

            //Debug.Print(intersectionsToProcess.Count().ToString());

            double timeToIntersect = closestIntersection.Item1;
            Ball ballInter = closestIntersection.Item2;
            Wall wallInter = closestIntersection.Item3;

            // III. Move each ball
            //newBalls.ForEach(b => b.move(timeToIntersect));
            ballInter.move(timeToIntersect);

            // IV. Apply bounce function to collising ball
            ballInter.bounce(wallInter);
            ballInter.incrementActualTime(timeToIntersect);

            Debug.Print("new state");

            // V. Return new state
            return new State(newBalls, newWalls, startTime + timeToIntersect);
        }

        public IEnumerable<Ball> after(double time)
        {
            return balls.Select(b => b.clone().move(time));
        }

        public void render(Graphics g, double nowTime, Color? ballsColor = null, bool startState = false)
        {
            Color color = ballsColor ?? Color.Green;
            double coeff = startState ? 0 : 1;

            balls.ForEach((ball) => {
                double timeDelta = (nowTime - ball.actualTime);
                double x = ball.moment.x + ball.moment.dx * timeDelta * coeff;
                double y = ball.moment.y + ball.moment.dy * timeDelta * coeff;

                g.DrawEllipse(
                    new Pen(color, 2),
                    (float)x - (float)ball.frame.radius,
                    (float)y - (float)ball.frame.radius,
                    (float)ball.frame.radius * 2,
                    (float)ball.frame.radius * 2
                );
            });

            walls.ForEach((wall) =>
                g.DrawLine(
                    new Pen(Color.Black, 2),
                    (float)wall.x1,
                    (float)wall.y1,
                    (float)wall.x2,
                    (float)wall.y2
                )
            );
        }
    }
}
