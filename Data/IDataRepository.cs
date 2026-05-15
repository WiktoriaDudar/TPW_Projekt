using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IDataRepository
    {
        IList<IBall> Balls { get; }
        void AddBall(IBall ball);
        void Clear();
        IList<IBall> GetBallsSnapshot();
    }
}
