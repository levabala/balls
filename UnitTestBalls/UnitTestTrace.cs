using System;
using Balls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBalls
{
    [TestClass]
    public class UnitTestTrace
    {
        [TestMethod]
        public void TraceTest1()
        {
            Line l = new Line(0, 6, 5, 6);
            Ball b = new Ball(
                new Vector(2, 1, 0.5, 2),
                1
            );

            b.updateTrace();

            Point interP = new Point();
            b.trace.checkCollision(l, b.moment.getStart(), ref interP);

            Assert.AreEqual(interP, new Point(3.25, 6));
        }
    }
}
