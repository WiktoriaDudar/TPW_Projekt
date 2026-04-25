using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

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
            Assert.AreEqual("red", ball.Color);
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

        [TestMethod]
        public void Ball_RaisesNewPositionNotification()
        {
            IBall ball = new Ball(0, 0, 5, "red");
            IVector? receivedVector = null;

            ball.NewPositionNotification += (sender, vector) =>
            {
                receivedVector = vector;
            };

            ball.X = 10;
            ball.Y = 20;

            Assert.IsNotNull(receivedVector);
            Assert.AreEqual(10, receivedVector.x);
            Assert.AreEqual(20, receivedVector.y);
        }


        [TestClass]
        public class VectorTests
        {
            [TestMethod]
            public void Vector_StoresCorrectValues()
            {
                IVector v = new Vector(3.5, 7.2);

                Assert.AreEqual(3.5, v.x);
                Assert.AreEqual(7.2, v.y);
            }
        }

        [TestClass]
        public class DataRepositoryTests
        {
            [TestMethod]
            public void DataRepository_AddsBalls()
            {
                IDataRepository repo = new DataRepository();
                IBall ball = new Ball(1, 2, 3, "blue");

                repo.AddBall(ball);

                Assert.AreEqual(1, repo.Balls.Count);
                Assert.AreSame(ball, repo.Balls[0]);
            }

            [TestMethod]
            public void DataRepository_ClearsBalls()
            {
                IDataRepository repo = new DataRepository();
                repo.AddBall(new Ball(1, 2, 3, "blue"));
                repo.AddBall(new Ball(4, 5, 6, "red"));

                repo.Clear();

                Assert.AreEqual(0, repo.Balls.Count);
            }
        }
    }
}
