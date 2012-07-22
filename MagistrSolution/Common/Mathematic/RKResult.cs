namespace Common.Mathematic
{
    public sealed class RKResult
    {
        public RKResult(double x, Vector y)
        {
            X = x;
            Y = y;
        }

        public double X { get; private set; }
        public Vector Y { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", X, Y);
        }
    }
}