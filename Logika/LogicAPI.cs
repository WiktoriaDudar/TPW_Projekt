using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private readonly IDataRepository _repository;
        private readonly Random _random = new Random();

        private readonly Dictionary<IBall, (double vx, double vy)> _velocities =
            new Dictionary<IBall, (double vx, double vy)>();

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

            double radius = 10;
            double speed = 3.0;

            for (int i = 0; i < count; i++)
            {
                double x = radius + _random.NextDouble() * (width - 2 * radius);
                double y = radius + _random.NextDouble() * (height - 2 * radius);

                var ball = new Ball(x, y, radius, "red");
                _repository.AddBall(ball);

                double angle = _random.NextDouble() * 2 * Math.PI;
                double vx = Math.Cos(angle) * speed;
                double vy = Math.Sin(angle) * speed;

                _velocities[ball] = (vx, vy);
            }
        }

        public void UpdatePositions(double width, double height)
        {
            foreach (var ball in Balls)
            {
                double r = ball.Radius;
                var (vx, vy) = _velocities[ball];

                ball.X += vx;
                ball.Y += vy;

                if (ball.X - r < 0 || ball.X + r > width)
                {
                    vx = -vx;
                }

                if (ball.Y - r < 0 || ball.Y + r > height)
                {
                    vy = -vy;
                }

                _velocities[ball] = (vx, vy);
            }
        }
    }
}
