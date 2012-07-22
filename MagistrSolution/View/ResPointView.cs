namespace View
{
    internal class ResPointViewType1
    {
        public ResPointViewType1(double t, double z, int m1, int m2)
        {
            T = t;
            Z1 = z;
            M1 = m1;
            M2 = m2;
        }

        public double T { get; private set; }
        public double Z1 { get; private set; }
        public int M1 { get; set; }
        public int M2 { get; set; }
    }


    internal sealed class ResPointViewType2 : ResPointViewType1
    {
        public ResPointViewType2(double t, double z1, double z2, int m1, int m2)
            : base(t, z1, m1, m2)
        {
            Z2 = z2;
        }

        public double Z2 { get; private set; }
    }
}