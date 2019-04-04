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
        public List<Line> supportLines = new List<Line>();
        public List<Point> supportPoints= new List<Point>();

        public readonly int id;
        public double startTime;
        public List<Ball> balls;
        public List<Wall> walls;

        public State(List<Ball> balls, List<Wall> walls, double time, int id, bool absolutelyNew = false)
        {
            this.balls = balls;
            this.walls = walls;
            this.id = id;
            startTime = time;

            if (absolutelyNew)
                balls.ForEach(ball => ball.setActualTime(startTime));
        }

        public State calcNextState()
        {
            List<Ball> newBalls = balls.Select(b => b.clone()).ToList();

            supportLines.Clear();

            IEnumerable<Tuple<double, Point, Wall, Ball>> intersections = newBalls.Select(ball =>
            {
                Trace t = ball.updateTrace().trace;
                Point start = ball.moment.getStart();

                //supportLines.AddRange(t.lines);
                supportLines.Add(t.leftLine);

                IEnumerable<Tuple<double, Point, Wall>> wallsWithIntersection =
                    walls
                    .Select(wall =>
                    {
                        double timeLeftToIntersect;
                        Point interP = new Point();
                        Collision collision = t.checkCollision(wall.l, ball.moment.getStart(), ref interP);

                        switch (collision)
                        {
                            case Collision.NoCollision:
                                return null;

                            case Collision.FullCollision:
                                Line perpLine = ball.moment.clone().setAngle(
                                    new Vector(wall.l)
                                    .rotate(Math.PI / 2)
                                    .angle
                                ).getLine();

                                Point perpInterP = new Point();
                                Intersection.intersect(wall.l, perpLine, ref perpInterP);

                                Point virtualInterP = new Point();
                                Intersection.intersect(ball.moment.getLine(), wall.l, ref virtualInterP);

                                double intersectionDist = ball.moment.getStart().dist(virtualInterP);
                                double dist = ball.moment.getStart().dist(perpInterP);
                                double length = (ball.frame.radius * intersectionDist) / dist;
                                //double length = 0;

                                double distToBounce = intersectionDist - length;
                                timeLeftToIntersect = distToBounce / ball.moment.length;

                                return new Tuple<double, Point, Wall>(
                                    timeLeftToIntersect, interP, wall
                                );

                            case Collision.PartialCollision:
                                Point wallP1 = wall.l.getStart();
                                Point wallP2 = wall.l.getEnd();
                                Point ballPoint = ball.moment.getStart();

                                Point closestWallEnd = 
                                    ballPoint.dist(wallP1) < ballPoint.dist(wallP2) ? wallP1 : wallP2;

                                Line tangetLine = ball.moment.clone().setStart(closestWallEnd).getLine();

                                Point interP1 = new Point();
                                Point interP2 = new Point();
                                Collision anotherCollision = 
                                    Intersection.intersect(
                                        tangetLine, 
                                        ball.frame, 
                                        ref interP1, 
                                        ref interP2
                                    );

                                Point closestInterP;
                                if (anotherCollision == Collision.PartialCollision)
                                    closestInterP = interP1;
                                else
                                    closestInterP = 
                                        closestWallEnd.dist(interP1) < closestWallEnd.dist(interP2) ? 
                                        interP1 : 
                                        interP2;

                                timeLeftToIntersect = closestInterP.dist(closestWallEnd) / ball.moment.length;

                                return new Tuple<double, Point, Wall>(
                                    timeLeftToIntersect, interP, wall
                                );
                        }

                        return null;
                    })
                    .Where(p => p != null);

                if (wallsWithIntersection.Count() == 0)
                    return null;

                Tuple<double, Point, Wall> closestWall = wallsWithIntersection.Aggregate(
                    (acc, val) => val.Item1 < acc.Item1 ? val : acc
                );
                

                return new Tuple<double, Point, Wall, Ball>(
                    closestWall.Item1,
                    closestWall.Item2,
                    closestWall.Item3,
                    ball
                );
            }).Where(p => p != null);

            if (intersections.Count() == 0)
                return this;

            const double minDiff = 0.1;

            Tuple<double, Point, Wall, Ball> closestIntersection = intersections.Aggregate(
                (acc, val) => val.Item1 < acc.Item1 ? val : acc
            );                        

            List<Tuple<double, Point, Wall, Ball>> followingIntersectons = intersections.Where(
                t => Math.Abs(t.Item1 - closestIntersection.Item1) < minDiff
            ).ToList();

            double timeToIntersect = Math.Max(closestIntersection.Item1, -minDiff);
            Point pointInter = closestIntersection.Item2;
            Wall wallInter = closestIntersection.Item3;
            Ball ballInter = closestIntersection.Item4;
            
            newBalls.ForEach(ball => ball.move(timeToIntersect));            

            if (followingIntersectons.Count == 0)
                Debug.Print("what");

            followingIntersectons.ForEach(t => t.Item4.bounce(wallInter));
            //ballInter.bounce(wallInter);
            Debug.Print(followingIntersectons.Count.ToString());

            //Debug.Print("---");
            //Debug.Print(string.Join("\n", intersections.OrderBy(t => t.Item1).Select(t => t.Item1.ToString())));


            return new State(newBalls, walls, startTime + timeToIntersect, id + 1);
        }

        public State calcNextStateDEPRECATED()
        {
            List<Ball> newBalls = balls.Select(b => b.clone()).ToList();
            List<Wall> newWalls = walls.Select(w => w.clone()).ToList();

            supportLines.Clear();
            supportPoints.Clear();

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
            newBalls.ForEach(b => b.move(timeToIntersect));
            //ballInter.move(timeToIntersect);

            // IV. Apply bounce function to collising ball
            ballInter.bounce(wallInter);
            //ballInter.incrementActualTime(timeToIntersect);            

            // V. Return new state
            return new State(newBalls, newWalls, startTime + timeToIntersect, id + 1);
        }

        public IEnumerable<Ball> after(double time)
        {
            return balls.Select(b => b.clone().move(time));
        }

        public void render(
            Graphics g, double nowTime, bool supportGeometry, Color? ballsColor = null, 
            bool startState = false)
        {
            Color color = ballsColor ?? Color.Green;
            double coeff = startState ? 0 : 1;

            double timeDelta = (nowTime - startTime);
            balls.ForEach((ball) => {
                //double timeDelta = (nowTime - ball.actualTime);
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

            if (supportGeometry)
            { 
                supportLines.ForEach(line => line.render(g, Pens.DarkGoldenrod));
                supportPoints.ForEach(point => point.render(g, Brushes.DarkRed, 5));
            }
        }
    }
}
