using System;
using System.Collections.Generic;
using System.Linq;
using Balls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBalls
{
    [TestClass]
    public class UnitTestRoom
    {
        [TestMethod]
        public void StateTest1()
        {
            List<Ball> balls = new List<Ball>() { new Ball(new Vector(3, 2, 1, 1), 1) };
            List<Wall> walls = new List<Wall>() { new Wall(3, 6, 8, 6) };

            State s1 = new State(balls, walls, 1);
            Ball ballBefore = s1.balls.First();

            State s2 = s1.calcNextState();
            Ball ballAfter = s2.balls.First();

            State s3 = s2.calcNextState();
            Ball ballAfterAfter = s3.balls.First();

            Ball ballAt6 = s3.after(2).First();

            // assert first state
            Assert.AreEqual(ballBefore.moment.getStart(), new Point(3, 2));
            Assert.AreEqual(ballBefore.moment.dx, 1);
            Assert.AreEqual(ballBefore.moment.dy, 1);
            Assert.AreEqual(s1.startTime, 1);

            // assert second state
            Assert.AreEqual(ballAfter.moment.getStart(), new Point(6, 5));
            Assert.AreEqual(ballAfter.moment.dx, 1, 0.0001);
            Assert.AreEqual(ballAfter.moment.dy, -1, 0.0001);
            Assert.AreEqual(s2.startTime, 4);

            // assert third state
            Assert.AreEqual(ballAfterAfter.moment.getStart(), new Point(6, 5));
            Assert.AreEqual(s3.startTime, 4);

            // assert ball after 6 secs
            Assert.AreEqual(ballAt6.moment.getStart(), new Point(8, 3));
        }
    }
}
