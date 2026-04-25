using Logic;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LogicTest
{
    public class FakeRepository : IDataRepository
    {
        public IList<IBall> Balls { get; } = new List<IBall>();

        public void AddBall(IBall ball) => Balls.Add(ball);

        public void Clear() => Balls.Clear();
    }

    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void GenerateBalls_CreatesCorrectNumber()
        {
            ILogicAPI logic = new LogicAPI(new FakeRepository());

            logic.GenerateBalls(10, 500, 500);

            Assert.AreEqual(10, logic.Balls.Count);
        }

        [TestMethod]
        public void GenerateBalls_BallsAreWithinBounds()
        {
            ILogicAPI logic = new LogicAPI(new FakeRepository());

            logic.GenerateBalls(20, 400, 300);

            foreach (var ball in logic.Balls)
            {
                Assert.IsTrue(ball.X >= ball.Radius);
                Assert.IsTrue(ball.X <= 400 - ball.Radius);

                Assert.IsTrue(ball.Y >= ball.Radius);
                Assert.IsTrue(ball.Y <= 300 - ball.Radius);
            }
        }

        [TestMethod]
        public void UpdatePositions_ChangesBallPosition()
        {
            ILogicAPI logic = new LogicAPI(new FakeRepository());

            logic.GenerateBalls(1, 500, 500);

            double oldX = logic.Balls[0].X;
            double oldY = logic.Balls[0].Y;

            logic.UpdatePositions(500, 500);

            Assert.AreNotEqual(oldX, logic.Balls[0].X);
            Assert.AreNotEqual(oldY, logic.Balls[0].Y);
        }

        [TestMethod]
        public void Logic_UsesInjectedRepository()
        {
            FakeRepository repo = new FakeRepository();
            ILogicAPI logic = new LogicAPI(repo);

            logic.GenerateBalls(5, 300, 300);

            Assert.AreEqual(5, repo.Balls.Count);
        }
    }
}
