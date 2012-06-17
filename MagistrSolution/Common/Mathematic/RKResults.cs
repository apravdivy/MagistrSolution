using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mathematic
{
    public class RKResults : List<RKResult>
    {

        public double MaximumZ1()
        {
            List<double> list = new List<double>();
            this.ForEach(x => list.Add(x.Y[0]));
            return list.Max();
        }
        public double MinimumZ1()
        {
            List<double> list = new List<double>();
            this.ForEach(x => list.Add(x.Y[0]));

            return list.Min();
        }

        public double MaximumZ2()
        {
            List<double> list = new List<double>();
            this.ForEach(item => list.Add(item.Y[1]));
            return list.Max();
        }
        public double MinimumZ2()
        {
            List<double> list = new List<double>();
            this.ForEach(item => list.Add(item.Y[1]));

            return list.Min();
        }

        public double GetH()
        {
            return (this[1].X - this[0].X);
        }
    }
}
