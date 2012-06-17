using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace View
{
    internal class ResPointViewType1
    {
        public double T { get; private set; }
        public double Z1 { get; private set; }
        public int M1 { get;  set; }
        public int M2 { get;  set; }


        public ResPointViewType1(double t, double z, int m1, int m2)
        {
            this.T = t;
            this.Z1 = z;
            this.M1 = m1;
            this.M2 = m2;
        }
    }


    internal sealed class ResPointViewType2 : ResPointViewType1
    {
        public double Z2 { get; private set; }

        public ResPointViewType2(double t, double z1, double z2, int m1, int m2)
            : base(t, z1, m1, m2)
        {
            this.Z2 = z2;
        }
    }
}
