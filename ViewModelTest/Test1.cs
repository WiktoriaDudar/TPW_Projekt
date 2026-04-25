using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public sealed class ViewModelTests
    {
        [TestMethod]
        public void Start_CreatesCorrectNumberOfBalls()
        {
            IViewModel vm = new ViewModel.ViewModel();

            vm.Start(5, 500, 500);

            Assert.AreEqual(5, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_ClearsPreviousBalls()
        {
            IViewModel vm = new ViewModel.ViewModel();

            vm.Start(3, 500, 500);
            vm.Start(1, 500, 500);

            Assert.AreEqual(1, vm.Balls.Count);
        }


        [TestMethod]
        public void BallsCollection_IsNotNull()
        {
            IViewModel vm = new ViewModel.ViewModel();

            Assert.IsNotNull(vm.Balls);
        }
    }
}
