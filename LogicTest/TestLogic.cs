using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void GenerateBalls_CreatesCorrectNumber()
        {
            ILogicAPI logic = new LogicAPI();

            logic.GenerateBalls(10, 500, 500);

            Assert.AreEqual(10, logic.Balls.Count);
        }
    }
}
