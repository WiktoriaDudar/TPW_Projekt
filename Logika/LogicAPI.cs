using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private readonly IDataRepository _repository;
        private readonly object _lock = new object();
        private readonly Random _random = new Random();

        public double MaxX { get; private set; } = 1000;
        public double MaxY { get; private set; } = 1000;

        public IList<IBall> Balls => _repository.Balls;

        public LogicAPI(IDataRepository repository)
        {
            _repository = repository;
        }

        public LogicAPI() : this(new DataRepository()) { }

        public void SetBounds(double width, double height)
        {
            MaxX = width;
            MaxY = height;
        }

        public void GenerateBalls(int count)
        {
            lock (_lock)
            {
                _repository.Clear();

                for (int i = 0; i < count; i++)
                {
                    double diameter = 20;
                    double radius = diameter / 2;

                    double x = radius + _random.NextDouble() * (MaxX - diameter);
                    double y = radius + _random.NextDouble() * (MaxY - diameter);

                    double angle = _random.NextDouble() * 2 * Math.PI;
                    double speed = 3.0;

                    IVector velocity = new Vector(
                        Math.Cos(angle) * speed,
                        Math.Sin(angle) * speed
                    );

                    double mass = diameter;

                    var ball = new Ball(
                        x,
                        y,
                        diameter,
                        "red",
                        velocity,
                        mass
                    );

                    _repository.AddBall(ball);

                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            lock (_lock)
                            {
                                UpdateBallPosition(ball);
                            }

                            await Task.Delay(16);
                        }
                    });
                }

                Task.Run(async () =>
                {
                    while (true)
                    {
                        lock (_lock)
                        {
                            HandleCollisions(
                                _repository.GetBallsSnapshot()
                            );
                        }

                        await Task.Delay(16);
                    }
                });
            }
        }

        private void UpdateBallPosition(IBall ball)
        {
            ball.X += ball.Velocity.X;
            ball.Y += ball.Velocity.Y;

            double radius = ball.Diameter / 2;

            if (ball.X - radius < 0 || ball.X + radius > MaxX)
            {
                ball.Velocity = new Vector(
                    -ball.Velocity.X,
                    ball.Velocity.Y
                );
            }

            if (ball.Y - radius < 0 || ball.Y + radius > MaxY)
            {
                ball.Velocity = new Vector(
                    ball.Velocity.X,
                    -ball.Velocity.Y
                );
            }
        }

        private void HandleCollisions(IList<IBall> balls)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                for (int j = i + 1; j < balls.Count; j++)
                {
                    ResolveCollision(balls[i], balls[j]);
                }
            }
        }

        private void ResolveCollision(IBall a, IBall b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);

            double minDist =
                (a.Diameter / 2) +
                (b.Diameter / 2);

            if (distance == 0 || distance > minDist)
                return;

            double nx = dx / distance;
            double ny = dy / distance;

            double overlap = minDist - distance;

            a.X -= (overlap / 2) * nx;
            a.Y -= (overlap / 2) * ny;

            b.X += (overlap / 2) * nx;
            b.Y += (overlap / 2) * ny;

            double va =
                a.Velocity.X * nx +
                a.Velocity.Y * ny;

            double vb =
                b.Velocity.X * nx +
                b.Velocity.Y * ny;

            double ma = a.Mass;
            double mb = b.Mass;

            double vaNew =
                (va * (ma - mb) + 2 * mb * vb)
                / (ma + mb);

            double vbNew =
                (vb * (mb - ma) + 2 * ma * va)
                / (ma + mb);

            a.Velocity = new Vector(
                a.Velocity.X + (vaNew - va) * nx,
                a.Velocity.Y + (vaNew - va) * ny
            );

            b.Velocity = new Vector(
                b.Velocity.X + (vbNew - vb) * nx,
                b.Velocity.Y + (vbNew - vb) * ny
            );
        }

        public double GetBallX(int id) => Balls[id].X;
        public double GetBallY(int id) => Balls[id].Y;
    }
}