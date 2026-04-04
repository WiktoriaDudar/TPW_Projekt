using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataTest
{
    [TestClass]
    public class BallTests
    {
        [TestMethod]
        public void Ball_CreatesWithCorrectProperties()
        {
            IBall ball = new Ball(10, 20, 5, "red");

            Assert.AreEqual(10, ball.X);
            Assert.AreEqual(20, ball.Y);
            Assert.AreEqual(5, ball.Radius);
        }

        [TestMethod]
        public void Ball_PositionCanChange()
        {
            IBall ball = new Ball(0, 0, 5, "red");

            ball.X = 15;
            ball.Y = 25;

            Assert.AreEqual(15, ball.X);
            Assert.AreEqual(25, ball.Y);
        }
    }
}
