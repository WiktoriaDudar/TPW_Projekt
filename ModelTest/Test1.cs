using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Logic;

namespace ModelTest
{
    [TestClass]
    public sealed class ModelTests
    {
        [TestMethod]
        public void ModelAPI_CreatesLogicAPI()
        {
            IModelAPI model = new ModelAPI();

            Assert.IsNotNull(model.Logic);
        }

        [TestMethod]
        public void ModelAPI_LogicIsOfCorrectType()
        {
            IModelAPI model = new ModelAPI();

            Assert.IsInstanceOfType(model.Logic, typeof(ILogicAPI));
        }
    }
}
