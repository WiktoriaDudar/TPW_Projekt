using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Threading;
using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public class ViewModelTests
    {
        private IViewModel CreateViewModel()
        {
            return new ViewModel.ViewModel(Dispatcher.CurrentDispatcher);
        }

        [TestMethod]
        public void Balls_IsNotNull_AfterCreation()
        {
            var vm = CreateViewModel();

            Assert.IsNotNull(vm.Balls);
        }

        [TestMethod]
        public void Balls_IsEmpty_AfterCreation()
        {
            var vm = CreateViewModel();

            Assert.AreEqual(0, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_AddsRequestedNumberOfBalls()
        {
            var vm = CreateViewModel();

            vm.Start(5, 500, 500);

            Assert.AreEqual(5, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_WithZeroBalls_CreatesEmptyCollection()
        {
            var vm = CreateViewModel();

            vm.Start(0, 500, 500);

            Assert.AreEqual(0, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_ClearsPreviousBalls()
        {
            var vm = CreateViewModel();

            vm.Start(10, 500, 500);
            Assert.AreEqual(10, vm.Balls.Count);

            vm.Start(3, 500, 500);

            Assert.AreEqual(3, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_AllBallsAreAddedToCollection()
        {
            var vm = CreateViewModel();

            vm.Start(7, 500, 500);

            foreach (var ball in vm.Balls)
            {
                Assert.IsNotNull(ball);
            }
        }

        [TestMethod]
        public void UpdateWindowSize_DoesNotThrowException()
        {
            var vm = CreateViewModel();

            vm.UpdateWindowSize(800, 600);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Start_WithLargeNumberOfBalls_AddsAllBalls()
        {
            var vm = CreateViewModel();

            vm.Start(100, 1000, 1000);

            Assert.AreEqual(100, vm.Balls.Count);
        }
    }
}

