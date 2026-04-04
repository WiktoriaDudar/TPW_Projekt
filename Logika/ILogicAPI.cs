using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ILogicAPI
    {
        IList<IBall> Balls { get; }
        void GenerateBalls(int count, double width, double height);
        void UpdatePositions();
    }
}

