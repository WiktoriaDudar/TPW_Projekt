using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{

    public class DataRepository : IDataRepository
    {
        public IList<IBall> Balls { get; } = new List<IBall>();

        public void AddBall(IBall ball)
        {
            Balls.Add(ball);
        }

        public void Clear()
        {
            Balls.Clear();
        }
    }
}
