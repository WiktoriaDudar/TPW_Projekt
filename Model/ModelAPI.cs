using Data;
using Logic;

namespace Model
{
    public class ModelAPI : IModelAPI
    {
        public ILogicAPI Logic { get; }

        private double windowWidth;
        private double windowHeight;

        public ModelAPI()
        {
            var repository = new DataRepository();
            Logic = new LogicAPI(repository);
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
    }
}
