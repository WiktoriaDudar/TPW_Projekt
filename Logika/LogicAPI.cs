using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private readonly IDataRepository _repository;
        private readonly Random _random = new Random();

        private readonly Dictionary<IBall, (double dx, double dy)> _velocities =
            new Dictionary<IBall, (double dx, double dy)>();

        public IList<IBall> Balls => _repository.Balls;

        public LogicAPI(IDataRepository repository)
        {
            _repository = repository;
        }

        public LogicAPI()
        {
            _repository = new DataRepository();
        }


        public void GenerateBalls(int count, double width, double height)
        {
            _repository.Clear();
            _velocities.Clear();

            for (int i = 0; i < count; i++)
            {
                double radius = 10;
                double x = _random.NextDouble() * (width - 2 * radius) + radius;
                double y = _random.NextDouble() * (height - 2 * radius) + radius;

                var ball = new Ball(x, y, radius, "red");
                _repository.AddBall(ball);

                double dx = _random.NextDouble() * 4 - 2;
                double dy = _random.NextDouble() * 4 - 2;

                _velocities[ball] = (dx, dy);
            }
        }

        public void UpdatePositions()
        {
            foreach (var ball in Balls)
            {
                var (dx, dy) = _velocities[ball];

                ball.X += dx;
                ball.Y += dy;
            }
        }
    }
}


