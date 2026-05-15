using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ILogicAPI
    {
        IList<IBall> Balls { get; }

        void GenerateBalls(int count);

        void UpdatePositions();

        void SetBounds(double width, double height);

        double MaxX { get; }
        double MaxY { get; }

        double GetBallX(int id);
        double GetBallY(int id);
    }
}
