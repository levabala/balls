using System;
using Balls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBalls
{
    [TestClass]
    public class UnitTestLine
    {
        [TestMethod]
        public void TestMethodIsBetweenFirst()
        {
            Line l1 = new Line(1, 3, 3, 1);
            Line l2 = new Line(6, 1, 7, 4);
            Point p = new Point(4, 3);

            bool isBetween = p.isBetween(l1, l2);

            Assert.AreEqual(isBetween, true);
        }

        [TestMethod]
        public void TestMethodIsBetweenSecond()
        {
            Line l1 = new Line(1, 4, 3, 1);
            Line l2 = new Line(6, 1, 0, 4);
            Point p = new Point(4, 3);

            bool isBetween = p.isBetween(l1, l2);

            Assert.AreEqual(isBetween, false);
        }
    }
}
