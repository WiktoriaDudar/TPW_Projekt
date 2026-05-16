using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace ModelTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void ModelAPI_CreatesLogic()
        {
            IModelAPI model = new ModelAPI();
            Assert.IsNotNull(model.Logic);
        }

        [TestMethod]
        public void SetWindowSize_DoesNotThrow()
        {
            IModelAPI model = new ModelAPI();
            model.SetWindowSize(500, 500);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Logic_GeneratesBalls()
        {
            IModelAPI model = new ModelAPI();
            model.Logic.GenerateBalls(5);
            Assert.AreEqual(5, model.Logic.Balls.Count);
        }

        [TestMethod]
        public void GetBallX_ReturnsNumber()
        {
            IModelAPI model = new ModelAPI();
            model.SetWindowSize(500, 500);
            model.Logic.GenerateBalls(1);
            double x = model.GetBallX(0);
            Assert.IsTrue(x >= 0);
        }

        [TestMethod]
        public void GetBallY_ReturnsNumber()
        {
            IModelAPI model = new ModelAPI();
            model.SetWindowSize(500, 500);
            model.Logic.GenerateBalls(1);
            double y = model.GetBallY(0);
            Assert.IsTrue(y >= 0);
        }
    }
}