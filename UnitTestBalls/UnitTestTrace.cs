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
            Circle c = new Circle(2, 2, 1);
            Vector moveV = new Vector(0, 5);
            Rect trace = c.getMoveTrace(moveV);
            Rect roundedTrace = trace.processWith((p) => new Point(Math.Round(p.x, 3), Math.Round(p.y, 3)));

            Rect expectedTrace = new Rect(new Point(1, 2), new Point(3, 2), new Point(1, 7), new Point(3, 7));
            Assert.AreEqual(roundedTrace, expectedTrace);
        }
    }
}
