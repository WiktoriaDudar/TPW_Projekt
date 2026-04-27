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

        public double MaxX { get; private set; } = 1000;
        public double MaxY { get; private set; } = 1000;

        public IList<IBall> Balls => _repository.Balls;

        public LogicAPI(IDataRepository repository)
        {
            _repository = repository;
        }

        public LogicAPI()
        {
            _repository = new DataRepository();
        }

    
        public void GenerateBalls(int count)
        {
            GenerateBallsInternal(count, MaxX, MaxY);
        }

       
        private void GenerateBallsInternal(int count, double width, double height)
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

        public void UpdatePositions()
        {
            UpdatePositionsInternal(MaxX, MaxY);
        }

        public void SetBounds(double width, double height)
        {
            MaxX = width;
            MaxY = height;
        }

        private void UpdatePositionsInternal(double width, double height)
        {
            foreach (var ball in Balls)
            {
                double r = ball.Radius;
                var (vx, vy) = _velocities[ball];

                ball.X += vx;
                ball.Y += vy;

                if (ball.X - r < 0 || ball.X + r > width)
                    vx = -vx;

                if (ball.Y - r < 0 || ball.Y + r > height)
                    vy = -vy;

                _velocities[ball] = (vx, vy);
            }
        }

        public double GetBallX(int id) => Balls[id].X;
        public double GetBallY(int id) => Balls[id].Y;
    }
}
