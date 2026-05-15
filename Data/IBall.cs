using System;
using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        double X { get; set; }
        double Y { get; set; }

        double Diameter { get; }
        double Radius { get; }

        string Color { get; }

        IVector Velocity { get; set; }
        double Mass { get; }

        event EventHandler<IVector>? NewPositionNotification;
    }
}
