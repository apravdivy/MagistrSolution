using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mathematic
{
    public class RKResult
    {
        public double X { get; set; }
        public Vector Y { get; set; }
        public RKResult(double x, Vector y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
