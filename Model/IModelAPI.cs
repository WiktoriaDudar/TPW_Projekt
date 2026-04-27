using Logic;

namespace Model
{
    public interface IModelAPI
    {
        ILogicAPI Logic { get; }
        void SetWindowSize(double width, double height);
        double GetBallX(int id);
        double GetBallY(int id);
    }
}
