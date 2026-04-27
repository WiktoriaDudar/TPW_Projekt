using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ILogicAPI
    {
        IList<IBall> Balls { get; }

        void GenerateBalls(int count);

        void UpdatePositions();

        double MaxX { get; }
        double MaxY { get; }

        double GetBallX(int id);
        double GetBallY(int id);
        void SetBounds(double width, double height);
    }
}
