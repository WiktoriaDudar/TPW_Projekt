using System;
using System.Collections.Generic;
using System.Threading;
using Data;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private readonly IDataRepository _repo;
        private readonly object _lock = new object();
        private Thread? _logicThread;
        private bool _running = false;

        public double MaxX { get; private set; } = 1000;
        public double MaxY { get; private set; } = 1000;

        public IList<IBall> Balls => _repo.Balls;

        public LogicAPI(IDataRepository repo)
        {
            _repo = repo;
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
                _repo.Clear();
                Random rnd = new Random();

                for (int i = 0; i < count; i++)
                {
                    double diameter = 20;
                    double radius = diameter / 2;

                    double x = radius + rnd.NextDouble() * (MaxX - diameter);
                    double y = radius + rnd.NextDouble() * (MaxY - diameter);

                    double angle = rnd.NextDouble() * 2 * Math.PI;
                    double speed = 3.0;

                    IVector velocity = new Vector(
                        Math.Cos(angle) * speed,
                        Math.Sin(angle) * speed
                    );

                    IBall ball = new Ball(x, y, diameter, "red", velocity, diameter);
                    _repo.AddBall(ball);
                }
            }

            StartLogicLoop();
        }

        private void StartLogicLoop()
        {
            if (_running)
                return;

            _running = true;

            _logicThread = new Thread(() =>
            {
                while (_running)
                {
                    Step();
                    Thread.Sleep(16);
                }
            });

            _logicThread.IsBackground = true;
            _logicThread.Start();
        }

        private void Step()
        {
            IList<IBall> snapshot;

            lock (_lock)
            {
                snapshot = _repo.GetBallsSnapshot();
            }

            foreach (var ball in snapshot)
                UpdateBall(ball);

            HandleCollisions(snapshot);
        }

        private void UpdateBall(IBall ball)
        {
            double r = ball.Diameter / 2;

            ball.X += ball.Velocity.X;
            ball.Y += ball.Velocity.Y;

            if (ball.X - r < 0 || ball.X + r > MaxX)
                ball.Velocity = new Vector(-ball.Velocity.X, ball.Velocity.Y);

            if (ball.Y - r < 0 || ball.Y + r > MaxY)
                ball.Velocity = new Vector(ball.Velocity.X, -ball.Velocity.Y);
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

            double dist = Math.Sqrt(dx * dx + dy * dy);
            double minDist = a.Radius + b.Radius;

            if (dist == 0 || dist > minDist)
                return;

            double nx = dx / dist;
            double ny = dy / dist;

            double overlap = minDist - dist;

            a.X -= nx * overlap / 2;
            a.Y -= ny * overlap / 2;

            b.X += nx * overlap / 2;
            b.Y += ny * overlap / 2;

            double va = a.Velocity.X * nx + a.Velocity.Y * ny;
            double vb = b.Velocity.X * nx + b.Velocity.Y * ny;

            double ma = a.Mass;
            double mb = b.Mass;

            double vaNew = (va * (ma - mb) + 2 * mb * vb) / (ma + mb);
            double vbNew = (vb * (mb - ma) + 2 * ma * va) / (ma + mb);

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
