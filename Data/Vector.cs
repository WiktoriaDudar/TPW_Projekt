using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Vector : IVector
    {
        public double x { get; }
        public double y { get; }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

