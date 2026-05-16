using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace DataTest
{


    [TestClass]
    public class BallTests
    {
        private class TestVector : IVector
        {
            public double X { get; }
            public double Y { get; }
            public double Length => Math.Sqrt(X * X + Y * Y);
            public TestVector(double x, double y)
            {
                X = x;
                Y = y;
            }

            public IVector Normalize() => this;
            public IVector Add(IVector other) => new TestVector(X + other.X, Y + other.Y);
            public IVector Subtract(IVector other) => new TestVector(X - other.X, Y - other.Y);
            public IVector Multiply(double scalar) => new TestVector(X * scalar, Y * scalar);
        }

        [TestMethod]
        public void Ball_CreatesWithCorrectProperties()
        {
            var vel = new TestVector(1, 2);
            IBall ball = new Ball(10, 20, 8, "red", vel, 3.5);
            Assert.AreEqual(10, ball.X);
            Assert.AreEqual(20, ball.Y);
            Assert.AreEqual(8, ball.Diameter);
            Assert.AreEqual(4, ball.Radius);
            Assert.AreEqual("red", ball.Color);
            Assert.AreEqual(vel, ball.Velocity);
            Assert.AreEqual(3.5, ball.Mass);
        }

        [TestMethod]
        public void Ball_PositionChange_RaisesPropertyChanged()
        {
            var vel = new TestVector(0, 0);
            IBall ball = new Ball(0, 0, 10, "blue", vel, 1);
            string? changedProp = null;
            ball.PropertyChanged += (s, e) => changedProp = e.PropertyName;
            ball.X = 50;
            Assert.AreEqual("X", changedProp);
        }

        [TestMethod]
        public void Ball_VelocityChange_RaisesPropertyChanged()
        {
            var vel = new TestVector(0, 0);
            IBall ball = new Ball(0, 0, 10, "blue", vel, 1);
            string? changedProp = null;
            ball.PropertyChanged += (s, e) => changedProp = e.PropertyName;
            ball.Velocity = new TestVector(5, 5);
            Assert.AreEqual("Velocity", changedProp);
        }

        [TestMethod]
        public void Ball_PositionChange_RaisesNewPositionNotification_WithVelocitySnapshot()
        {
            var vel = new TestVector(3, 4);
            IBall ball = new Ball(0, 0, 10, "blue", vel, 1);

            IVector? received = null;
            ball.NewPositionNotification += (s, v) => received = v;
            ball.X = 100;
            Assert.IsNotNull(received);
            Assert.AreEqual(3, received.X);
            Assert.AreEqual(4, received.Y);
        }

        [TestMethod]
        public void Ball_ThreadSafety_GettersReturnConsistentValues()
        {
            var vel = new TestVector(1, 1);
            IBall ball = new Ball(10, 20, 10, "green", vel, 1);

            for (int i = 0; i < 1000; i++)
            {
                _ = ball.X;
                _ = ball.Y;
                _ = ball.Velocity;
            }

            Assert.AreEqual(10, ball.X);
            Assert.AreEqual(20, ball.Y);
            Assert.AreEqual(vel, ball.Velocity);
        }
    }


    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Vector_StoresCorrectValues()
        {
            IVector v = new Vector(3.5, 7.2);
            Assert.AreEqual(3.5, v.X);
            Assert.AreEqual(7.2, v.Y);
        }

        [TestMethod]
        public void Vector_LengthIsCorrect()
        {
            IVector v = new Vector(3, 4);
            Assert.AreEqual(5, v.Length);
        }

        [TestMethod]
        public void Vector_Normalize_Works()
        {
            IVector v = new Vector(3, 4);
            IVector n = v.Normalize();
            Assert.AreEqual(0.6, n.X, 0.0001);
            Assert.AreEqual(0.8, n.Y, 0.0001);
        }

        [TestMethod]
        public void Vector_Add_Works()
        {
            IVector v1 = new Vector(1, 2);
            IVector v2 = new Vector(3, 4);
            IVector r = v1.Add(v2);
            Assert.AreEqual(4, r.X);
            Assert.AreEqual(6, r.Y);
        }

        [TestMethod]
        public void Vector_Subtract_Works()
        {
            IVector v1 = new Vector(5, 5);
            IVector v2 = new Vector(2, 3);
            IVector r = v1.Subtract(v2);
            Assert.AreEqual(3, r.X);
            Assert.AreEqual(2, r.Y);
        }

        [TestMethod]
        public void Vector_Multiply_Works()
        {
            IVector v = new Vector(2, 3);
            IVector r = v.Multiply(2);
            Assert.AreEqual(4, r.X);
            Assert.AreEqual(6, r.Y);
        }
    }

    [TestClass]
    public class DataRepositoryTests
    {
        private class DummyBall : IBall
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Diameter => 10;
            public double Radius => 5;
            public string Color => "x";
            public IVector Velocity { get; set; } = new Vector(0, 0);
            public double Mass => 1;

            public event PropertyChangedEventHandler? PropertyChanged;
            public event EventHandler<IVector>? NewPositionNotification;
        }

        [TestMethod]
        public void AddBall_AddsBallToList()
        {
            IDataRepository repo = new DataRepository();
            IBall ball = new DummyBall();
            repo.AddBall(ball);
            Assert.AreEqual(1, repo.Balls.Count);
            Assert.AreSame(ball, repo.Balls[0]);
        }

        [TestMethod]
        public void Clear_RemovesAllBalls()
        {
            IDataRepository repo = new DataRepository();
            repo.AddBall(new DummyBall());
            repo.AddBall(new DummyBall());
            repo.Clear();
            Assert.AreEqual(0, repo.Balls.Count);
        }

        [TestMethod]
        public void GetBallsSnapshot_ReturnsCopy()
        {
            IDataRepository repo = new DataRepository();
            var b1 = new DummyBall();
            repo.AddBall(b1);
            var snapshot = repo.GetBallsSnapshot();
            Assert.AreEqual(1, snapshot.Count);
            Assert.AreNotSame(repo.Balls, snapshot);
        }
    }
}
