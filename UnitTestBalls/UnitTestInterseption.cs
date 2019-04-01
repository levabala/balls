using System;
using Balls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBalls
{
    [TestClass]
    public class UnitTestInterseption
    {
        [TestMethod]
        public void Line2LineTest1()
        {
            Line l1 = new Line(1, 1, 5, 3);
            Line l2 = new Line(1, 3, 5, 1);

            Point interP = new Point();
            bool intersepted = Intersection.intersect(l1, l2, ref interP);


            Assert.AreEqual(interP, new Point(3, 2));
            Assert.IsTrue(intersepted);
        }

        [TestMethod]
        public void Line2LineTest2()
        {
            Line l1 = new Line(1, 1, 5, 3);
            Line l2 = new Line(2, 2, 6, 4);

            Point interP = new Point();
            bool intersepted = Intersection.intersect(l1, l2, ref interP);


            Assert.IsFalse(intersepted);
        }

        [TestMethod]
        public void Line2CircleTest1()
        {
            Line l1 = new Line(1, 1, 5, 3);
            Circle c1 = new Circle(8, 2, 1);

            Point interP = new Point();
            bool intersepted = Intersection.intersect(l1, c1, ref interP);

            Assert.IsFalse(intersepted);
        }

        [TestMethod]
        public void Line2CircleTest2()
        {
            Line l1 = new Line(1, 1, 5, 3);
            Circle c1 = new Circle(4, 2, 2);

            Point interP = new Point();
            bool intersepted = Intersection.intersect(l1, c1, ref interP);

            Assert.IsTrue(intersepted);
        }
    }
}
