using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DataRepository : IDataRepository
    {
        private readonly object _lock = new object();

        public IList<IBall> Balls { get; } = new List<IBall>();

        public void AddBall(IBall ball)
        {
            lock (_lock)
            {
                Balls.Add(ball);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                Balls.Clear();
            }
        }

        public IList<IBall> GetBallsSnapshot()
        {
            lock (_lock)
            {
                return Balls.ToList(); 
            }
        }
    }
}

