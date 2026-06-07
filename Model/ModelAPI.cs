using Logic;
using Data;

namespace Model
{
    public class ModelAPI : IModelAPI
    {
        public ILogicAPI Logic { get; }

        private double windowWidth;
        private double windowHeight;

        public ModelAPI(ILogicAPI logic)
        {
            Logic = logic;
        }

        public ModelAPI()
            : this(new LogicAPI(new DataRepository()))
        {
        }

        public void SetWindowSize(double width, double height)
        {
            windowWidth = width;
            windowHeight = height;
            Logic.SetBounds(width, height);
        }

        public double GetBallX(int id)
        {
            return (Logic.GetBallX(id) / Logic.MaxX) * windowWidth;
        }

        public double GetBallY(int id)
        {
            return (Logic.GetBallY(id) / Logic.MaxY) * windowHeight;
        }

        public double GetBallDiameter(int id)
        {
            return (Logic.Balls[id].Diameter / Logic.MaxX) * windowWidth;
        }
    }
}