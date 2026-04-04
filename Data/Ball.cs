using System.Collections.Generic;

namespace Data
{
  
    public class Ball : IBall
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; }
        public string Color { get; }

        public Ball(double x, double y, double radius, string color)
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
        }
    }

}

