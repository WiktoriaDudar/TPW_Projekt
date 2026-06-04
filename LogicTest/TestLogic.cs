using Logic;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LogicTest
{
    public class FakeRepository : IDataRepository
    {
        public IList<IBall> Balls { get; } = new List<IBall>();
        public void AddBall(IBall ball) => Balls.Add(ball);
        public void Clear() => Balls.Clear();
        public IList<IBall> GetBallsSnapshot() => new List<IBall>(Balls);
    }

    [TestClass]
    public class LogicAPITests
    {
        private void InvokePrivateVoid(object obj, string method, params object[] args)
        {
            var m = obj.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance);
            m.Invoke(obj, args);
        }

        [TestMethod]
        public void GenerateBalls_CreatesCorrectNumber()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);
            logic.GenerateBalls(10);
            Assert.AreEqual(10, logic.Balls.Count);
        }

        [TestMethod]
        public void GenerateBalls_BallsHaveCorrectMassAndVelocity()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);
            logic.GenerateBalls(5);

            foreach (var b in logic.Balls)
            {
                Assert.AreEqual(20, b.Diameter);
                Assert.AreEqual(20, b.Mass);
                Assert.AreEqual(200, b.Velocity.Length, 0.001);
            }
        }

        [TestMethod]
        public void GenerateBalls_BallsAreWithinBounds()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);
            logic.SetBounds(500, 300);
            logic.GenerateBalls(20);

            foreach (var ball in logic.Balls)
            {
                Assert.IsTrue(ball.X >= ball.Radius);
                Assert.IsTrue(ball.X <= logic.MaxX - ball.Radius);
                Assert.IsTrue(ball.Y >= ball.Radius);
                Assert.IsTrue(ball.Y <= logic.MaxY - ball.Radius);
            }
        }

        [TestMethod]
        public void UpdateBallPosition_ReflectsFromWalls()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);

            var ball = new Ball(1,5.0, 5.0, 20.0, "red", new Vector(-200, -200), 20.0);
            repo.AddBall(ball);

            InvokePrivateVoid(logic, "UpdateBallPosition", ball, 0.1);

            Assert.IsTrue(ball.Velocity.X > 0);
            Assert.IsTrue(ball.Velocity.Y > 0);
        }

        [TestMethod]
        public void ResolveCollision_ChangesVelocities()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);

            var a = new Ball(1,50.0, 50.0, 20.0, "red", new Vector(200, 0), 20.0);
            var b = new Ball(2,60.0, 50.0, 20.0, "red", new Vector(-200, 0), 20.0);

            repo.AddBall(a);
            repo.AddBall(b);

            InvokePrivateVoid(logic, "ResolveCollision", a, b);

            Assert.AreNotEqual(200, a.Velocity.X);
            Assert.AreNotEqual(-200, b.Velocity.X);
        }

        [TestMethod]
        public void Logic_UsesInjectedRepository()
        {
            var repo = new FakeRepository();
            var logic = new LogicAPI(repo);
            logic.GenerateBalls(5);
            Assert.AreEqual(5, repo.Balls.Count);
        }
    }
}
