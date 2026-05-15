using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Vector : IVector
    {
        public double X { get; }
        public double Y { get; }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public IVector Normalize()
        {
            double len = Length;
            if (len == 0) return new Vector(0, 0);
            return new Vector(X / len, Y / len);
        }

        public IVector Add(IVector other)
        {
            return new Vector(X + other.X, Y + other.Y);
        }

        public IVector Subtract(IVector other)
        {
            return new Vector(X - other.X, Y - other.Y);
        }

        public IVector Multiply(double scalar)
        {
            return new Vector(X * scalar, Y * scalar);
        }
    }
}


