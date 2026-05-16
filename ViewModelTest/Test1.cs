using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Threading;
using ViewModel;

namespace ViewModelTest
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void Balls_IsNotNull()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            IViewModel vm = new ViewModel.ViewModel(dispatcher);
            Assert.IsNotNull(vm.Balls);
        }

        [TestMethod]
        public void Start_AddsBalls()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            IViewModel vm = new ViewModel.ViewModel(dispatcher);
            vm.Start(5, 500, 500);
            Assert.AreEqual(5, vm.Balls.Count);
        }

        [TestMethod]
        public void Start_WithZeroBalls_CreatesEmptyCollection()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            IViewModel vm = new ViewModel.ViewModel(dispatcher);
            vm.Start(0, 500, 500);
            Assert.AreEqual(0, vm.Balls.Count);
        }
    }
}