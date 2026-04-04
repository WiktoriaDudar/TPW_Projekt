using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {

            IViewModel vm = new ViewModel.ViewModel();

            vm.Start(5, 500, 500);

            Assert.AreEqual(5, vm.Balls.Count);
        }
    }
}
