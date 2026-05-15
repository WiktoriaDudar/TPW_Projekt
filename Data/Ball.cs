using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        private readonly object _lock = new object();

        private double x;
        private double y;
        private IVector velocity;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<IVector>? NewPositionNotification;

        public double X
        {
            get
            {
                lock (_lock)
                    return x;
            }
            set
            {
                IVector velSnapshot;
                lock (_lock)
                {
                    x = value;
                    velSnapshot = velocity;
                }

                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, velSnapshot);
            }
        }

        public double Y
        {
            get
            {
                lock (_lock)
                    return y;
            }
            set
            {
                IVector velSnapshot;
                lock (_lock)
                {
                    y = value;
                    velSnapshot = velocity;
                }

                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, velSnapshot);
            }
        }

        public double Diameter { get; }
        public double Radius => Diameter / 2;

        public string Color { get; }

        public IVector Velocity
        {
            get
            {
                lock (_lock)
                    return velocity;
            }
            set
            {
                lock (_lock)
                {
                    velocity = value;
                }
                OnPropertyChanged();
            }
        }

        public double Mass { get; }

        public Ball(double x, double y, double diameter, string color, IVector velocity, double mass)
        {
            this.x = x;
            this.y = y;
            Diameter = diameter;
            Color = color;
            this.velocity = velocity;
            Mass = mass;
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
