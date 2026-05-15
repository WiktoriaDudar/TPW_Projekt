using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall
    {
        private double x;
        private double y;
        private IVector velocity;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<IVector>? NewPositionNotification;

        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, Velocity);
            }
        }

        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, Velocity);
            }
        }

        public double Diameter { get; }
        public double Radius => Diameter/2;

        public string Color { get; }

        public IVector Velocity
        {
            get => velocity;
            set
            {
                velocity = value;
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
            Velocity = velocity;
            Mass = mass;
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
