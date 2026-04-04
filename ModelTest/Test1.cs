using Model;

namespace ModelTest
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IModelAPI model = new ModelAPI();

            Assert.IsNotNull(model.Logic);
        }
    }
}
