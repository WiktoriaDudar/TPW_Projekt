using Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public class LogicAPI : ILogicAPI
    {
        private readonly IDataRepository _repository;
        private readonly object _lock = new object();
        private static int _nextId = 0;
        private readonly Random _random = new Random();

        private readonly ConcurrentQueue<BallLogEntry> _logQueue = new();
        private readonly SemaphoreSlim _fileLock = new(1, 1);

        private Timer _timer;
        private DateTime _lastTime;
        private int _isUpdating = 0; 
        private const int TimerIntervalMs = 10;

        public double MaxX { get; private set; } = 1000;
        public double MaxY { get; private set; } = 1000;

        public IList<IBall> Balls => _repository.Balls;

        private int _running = 0;

        public LogicAPI(IDataRepository repository)
        {
            _repository = repository;
        }

        public LogicAPI() : this(new DataRepository()) { }

        public void SetBounds(double width, double height)
        {
            lock (_lock)
            {
                MaxX = width;
                MaxY = height;
            }
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
                    double speed = 200.0;

                    var velocity = new Vector(
                        Math.Cos(angle) * speed,
                        Math.Sin(angle) * speed
                    );

                    var ball = new Ball(
                        _nextId++,
                        x,
                        y,
                        diameter,
                        "red",
                        velocity,
                        diameter
                    );

                    _repository.AddBall(ball);
                }
            }

            StartSimulation();
            StartLoggingThread();
        }

        public void StartSimulation()
        {
            if (Interlocked.Exchange(ref _running, 1) == 1)
                return;

            _lastTime = DateTime.UtcNow;
            _timer = new Timer(SimulationTick, null, 0, TimerIntervalMs);
        }

        public void StopSimulation()
        {
            if (Interlocked.Exchange(ref _running, 0) == 0)
                return;
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
        }

        private void SimulationTick(object state)
        {
            if (Interlocked.Exchange(ref _isUpdating, 1) == 1)
                return;

            try
            {
                if (Volatile.Read(ref _running) == 0)
                    return;

                DateTime now = DateTime.UtcNow;
                double dt = (now - _lastTime).TotalSeconds;
                _lastTime = now;

                List<IBall> snapshot;
                lock (_lock)
                {
                    snapshot = _repository.GetBallsSnapshot().ToList();
                }

                foreach (var ball in snapshot)
                {
                    UpdateBallPosition(ball, dt);
                }

                for (int i = 0; i < snapshot.Count; i++)
                {
                    HandleCollisions(snapshot, i);
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isUpdating, 0); 
            }
        }

        private void UpdateBallPosition(IBall ball, double dt)
        {
            ball.X += ball.Velocity.X * dt;
            ball.Y += ball.Velocity.Y * dt;

            double r = ball.Diameter / 2;

            if (ball.X - r < 0)
            {
                ball.X = r;
                ball.Velocity = new Vector(-ball.Velocity.X, ball.Velocity.Y);
            }

            if (ball.X + r > MaxX)
            {
                ball.X = MaxX - r;
                ball.Velocity = new Vector(-ball.Velocity.X, ball.Velocity.Y);
            }

            if (ball.Y - r < 0)
            {
                ball.Y = r;
                ball.Velocity = new Vector(ball.Velocity.X, -ball.Velocity.Y);
            }

            if (ball.Y + r > MaxY)
            {
                ball.Y = MaxY - r;
                ball.Velocity = new Vector(ball.Velocity.X, -ball.Velocity.Y);
            }

            LogBall(ball);
        }

        private void HandleCollisions(IList<IBall> balls, int i)
        {
            for (int j = i + 1; j < balls.Count; j++)
                ResolveCollision(balls[i], balls[j]);
        }

        private void ResolveCollision(IBall a, IBall b)
        {
            double dx = b.X - a.X;
            double dy = b.Y - a.Y;

            double distSq = dx * dx + dy * dy;
            if (distSq == 0) return;

            double distance = Math.Sqrt(distSq);
            double minDist = (a.Diameter / 2) + (b.Diameter / 2);

            if (distance > minDist)
                return;

            double nx = dx / distance;
            double ny = dy / distance;

            double relVel =
                (b.Velocity.X - a.Velocity.X) * nx +
                (b.Velocity.Y - a.Velocity.Y) * ny;

            if (relVel > 0)
                return;

            double overlap = minDist - distance;

            a.X -= (overlap / 2) * nx;
            a.Y -= (overlap / 2) * ny;

            b.X += (overlap / 2) * nx;
            b.Y += (overlap / 2) * ny;

            double va = a.Velocity.X * nx + a.Velocity.Y * ny;
            double vb = b.Velocity.X * nx + b.Velocity.Y * ny;

            double ma = a.Mass;
            double mb = b.Mass;

            double vaNew = (va * (ma - mb) + 2 * mb * vb) / (ma + mb);
            double vbNew = (vb * (mb - ma) + 2 * ma * va) / (ma + mb);

            Vector newA = new(
                a.Velocity.X + (vaNew - va) * nx,
                a.Velocity.Y + (vaNew - va) * ny
            );

            Vector newB = new(
                b.Velocity.X + (vbNew - vb) * nx,
                b.Velocity.Y + (vbNew - vb) * ny
            );

            const double targetSpeed = 200.0;

            a.Velocity = Normalize(newA, targetSpeed);
            b.Velocity = Normalize(newB, targetSpeed);

            LogBall(a);
            LogBall(b);
        }

        private Vector Normalize(Vector v, double targetSpeed)
        {
            double len = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (len == 0) return v;

            return new Vector(v.X / len * targetSpeed, v.Y / len * targetSpeed);
        }

        private void LogBall(IBall ball)
        {
            _logQueue.Enqueue(new BallLogEntry
            {
                Time = DateTime.UtcNow,
                Id = ball.Id,
                X = ball.X,
                Y = ball.Y,
                Vx = ball.Velocity.X,
                Vy = ball.Velocity.Y
            });
        }

        private void StartLoggingThread()
        {
            Task.Run(async () =>
            {
                using var writer = new StreamWriter("balls_log.txt", true, Encoding.ASCII);

                while (Volatile.Read(ref _running) == 1 || !_logQueue.IsEmpty)
                {
                    if (_logQueue.TryDequeue(out var log))
                    {
                        await _fileLock.WaitAsync();
                        try
                        {
                            await writer.WriteLineAsync(log.ToString());
                        }
                        finally
                        {
                            _fileLock.Release();
                        }
                    }
                    else
                    {
                        await Task.Delay(10);
                    }
                }
            });
        }

        public class BallLogEntry
        {
            public DateTime Time { get; set; }
            public int Id { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Vx { get; set; }
            public double Vy { get; set; }

            public override string ToString()
                => $"{Time:o};{Id};{X};{Y};{Vx};{Vy}";
        }

        public double GetBallX(int id) => Balls[id].X;
        public double GetBallY(int id) => Balls[id].Y;
    }
}