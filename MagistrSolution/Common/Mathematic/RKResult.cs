using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mathematic
{
    public sealed class RKResult
    {
        public double X { get; private set; }
        public Vector Y { get; private set; }

        public RKResult(double x, Vector y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }

    }
}
