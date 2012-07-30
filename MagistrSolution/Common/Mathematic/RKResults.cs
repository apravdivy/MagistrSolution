using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Mathematic
{
    public sealed class RKResults : List<RKResult>
    {
        public double MaximumZ1()
        {
            var list = new List<double>();
            ForEach(x => list.Add(x.Y[0]));
            return list.Max();
        }

        public double MinimumZ1()
        {
            var list = new List<double>();
            ForEach(x => list.Add(x.Y[0]));

            return list.Min();
        }

        public double MaximumZ2()
        {
            var list = new List<double>();
            ForEach(item => list.Add(item.Y[1]));
            return list.Max();
        }

        public double MinimumZ2()
        {
            var list = new List<double>();
            ForEach(item => list.Add(item.Y[1]));

            return list.Min();
        }

        public double GetH()
        {
            return (this[1].X - this[0].X);
        }

        public RKResults Trancate(int step)
        {
            if (step == 0)
            {
                return this;
            }
            if (Count > 1)
            {
                var res = new RKResults();
                int i = step;
                foreach (RKResult r in this)
                {
                    if (i == step)
                    {
                        res.Add(r);
                        i--;
                    }
                    else
                    {
                        if (i <= 0)
                        {
                            i = step;
                        }
                        else
                        {
                            i--;
                        }
                    }
                }
                return res;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}