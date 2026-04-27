using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : IBall, INotifyPropertyChanged
    {
        private double x;
        private double y;

        public event EventHandler<IVector>? NewPositionNotification;
        public event PropertyChangedEventHandler? PropertyChanged;

        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, new Vector(x, y));
            }
        }

        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged();
                NewPositionNotification?.Invoke(this, new Vector(x, y));
            }
        }

        public double Radius { get; }
        public string Color { get; }

        public double Diameter => Radius * 2;

        public Ball(double x, double y, double radius, string color)
        {
            this.x = x;
            this.y = y;
            Radius = radius;
            Color = color;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
