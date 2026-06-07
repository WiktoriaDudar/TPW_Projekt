using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Constructor_AssignsLogic()
        {
            ILogicAPI fakeLogic = new FakeLogic();

            IModelAPI model = new ModelAPI(fakeLogic);

            Assert.IsNotNull(model.Logic);
            Assert.AreSame(fakeLogic, model.Logic);
        }

        [TestMethod]
        public void SetWindowSize_UpdatesLogicBounds()
        {
            var fakeLogic = new FakeLogic();

            IModelAPI model = new ModelAPI(fakeLogic);

            model.SetWindowSize(800, 600);

            Assert.AreEqual(800, fakeLogic.MaxX);
            Assert.AreEqual(600, fakeLogic.MaxY);
        }

        [TestMethod]
        public void GetBallX_ScalesCoordinatesCorrectly()
        {
            var fakeLogic = new FakeLogic();

            IModelAPI model = new ModelAPI(fakeLogic);

            model.SetWindowSize(2000, 1000);

            double x = model.GetBallX(0);

            Assert.AreEqual(500, x);
        }

        [TestMethod]
        public void GetBallY_ScalesCoordinatesCorrectly()
        {
            var fakeLogic = new FakeLogic();

            IModelAPI model = new ModelAPI(fakeLogic);

            model.SetWindowSize(1000, 1000);

            double y = model.GetBallY(0);

            Assert.AreEqual(250, y);
        }
    }
}



namespace ModelTest
{
    internal class FakeLogic : ILogicAPI
    {
        public IList<IBall> Balls { get; } = new List<IBall>();

        public double MaxX { get; private set; } = 1000;
        public double MaxY { get; private set; } = 1000;

        public void GenerateBalls(int count) { }

        public void SetBounds(double width, double height)
        {
            MaxX = width;
            MaxY = height;
        }

        public double GetBallX(int id) => 500;
        public double GetBallY(int id) => 250;

        public void StartSimulation() { }
        public void StopSimulation() { }
    }
}