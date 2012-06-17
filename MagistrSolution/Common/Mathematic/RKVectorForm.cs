using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;

namespace Common.Mathematic
{
    public class RKVectorForm
    {
        protected double t0;
        protected double t1;
        protected Vector Y0;

        public Func<double, Vector, Vector> f;

        private IFunctionExecuter fe = null;

        protected int N;
        protected double eps;
        protected double eps1 = 0.000001;
        protected double h0 = 0.5;

        protected List<double> t;
        protected List<Vector> z;

        private string curveName;

        public event Action<RKResult, string> OnResultGenerated;

        public event Action<RKResults, string, IFunctionExecuter> OnSolvingDone;

        private void OnResultGeneratedCall(RKResult r)
        {
            if (this.OnResultGenerated != null)
            {
                this.OnResultGenerated(r, this.curveName);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="f">delegate</param>
        /// <param name="c">0-main, 1-left,2-right</param>
        private RKVectorForm(Func<double, Vector, Vector> f, string curveName)
        {
            this.f = f;
            this.curveName = curveName;
        }

        public RKVectorForm(IFunctionExecuter fe, string curveName)
            : this(new Func<double, Vector, Vector>(fe.AbstractFunction), curveName)
        {
            this.fe = fe;
        }

        public RKVectorForm(IFunctionExecuter fe, string curveName, double a, double b, Vector Y0)
            : this(fe, curveName)
        {
            this.SetAlgorithmParameters(a, b, Y0);
        }

        public void SetAlgorithmParameters(double a, double b, Vector Y0)
        {
            this.t0 = a;
            this.t1 = b;

            this.Y0 = Y0;
            t = new List<double>();
            z = new List<Vector>();
            N = Y0.Count;
        }

        protected virtual Vector RK4_1(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1, f2, f3;

            f0 = h * f(x, y);
            f1 = h * f(x + h / 2, y + f0 / 2);
            f2 = h * f(x + h / 2, y + f1 / 2);
            f3 = h * f(x + h, y + f2);
            res = y + (f0 + 2 * f1 + 2 * f2 + f3) / 6;

            return res;
        }

        //public static Vector RKMethod(double x, Vector y, double h,int n,)
        //{
        //    Vector r=new Vector
        //}

        protected Vector RK4_2(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1, f2, f3;
            f0 = h * f(x, y);
            f1 = h * f(x + h / 4, y + f0 / 4);
            f2 = h * f(x + h / 2, y + f1 / 2);
            f3 = h * f(x + h, y + f0 - 2 * f1 + 2 * f2);
            res = y + (f0 + 4 * f2 + f3) / 6;
            return res;
        }
        protected Vector RK3_1(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1, f2;
            f0 = h * f(x, y);
            f1 = h * f(x + h / 2, y + f0 / 2);
            f2 = h * f(x + h / 2, y - f0 + 2 * f1);
            res = y + (f0 + 4 * f1 + f2) / 6;
            return res;
        }
        protected Vector RK3_2(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1, f2;
            f0 = h * f(x, y);
            f1 = h * f(x + h / 3, y + f0 / 3);
            f2 = h * f(x + 2 * h / 3, y + 2 * f1 / 3);
            res = y + (f0 + 3 * f2) / 4;
            return res;
        }
        protected Vector RK2_1(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1;
            f0 = h * f(x, y);
            f1 = h * f(x + h, y + f0);
            res = y + (f0 + f1) / 2;
            return res;
        }

        protected Vector RK2_2(double x, Vector y, double h)
        {
            Vector res = new Vector(N);
            Vector f0, f1;
            f0 = h * f(x, y);
            f1 = h * f(x + h / 2, y + f0 / 2);

            res = y + f1;
            return res;
        }

        public static double H
        {
            get;
            private set;
        }

        public void SolveWithConstHAsync(object obj)
        {
            object[] p = obj as object[];
            int N = Convert.ToInt32(p[0]);
            RKMetodType type = (RKMetodType)p[1];
            this.SolveWithConstH(N, type);
        }
        public RKResults SolveWithConstH(int N, RKMetodType type)
        {
            RKResults res = new RKResults();
            t.Clear();
            z.Clear();
            Func<double, Vector, double, Vector> RKfunc = null;
            switch (type)
            {
                case RKMetodType.RK4_1: RKfunc = new Func<double, Vector, double, Vector>(RK4_1); break;
                case RKMetodType.RK4_2: RKfunc = new Func<double, Vector, double, Vector>(RK4_2); break;
                case RKMetodType.RK3_1: RKfunc = new Func<double, Vector, double, Vector>(RK3_1); break;
                case RKMetodType.RK3_2: RKfunc = new Func<double, Vector, double, Vector>(RK3_2); break;
                case RKMetodType.RK2_1: RKfunc = new Func<double, Vector, double, Vector>(RK2_1); break;
                case RKMetodType.RK2_2: RKfunc = new Func<double, Vector, double, Vector>(RK2_2); break;
            }
            double h = (this.t1 - this.t0) / N;
            RKVectorForm.H = h;
            double x;
            Vector y;
            t.Add(this.t0);
            z.Add(Y0);
            RKResult r = new RKResult(t0, Y0);
            res.Add(r);
            this.OnResultGeneratedCall(r);
            for (int i = 0; i < N; i++)
            {
                y = RKfunc(t[i], z[i], h);
                x = t0 + (i + 1) * h;
                t.Add(x);
                z.Add(y);
                r = new RKResult(x, y);
                res.Add(r);
                this.OnResultGeneratedCall(r);
            }
            if (this.OnSolvingDone != null)
            {
                this.OnSolvingDone(res, this.curveName, this.fe);
            }
            return res;
        }

        public void SolveWithAdaptiveH(int NumMet)
        {
            Func<double, Vector, double, Vector> RKfunc = null;
            int k = 0;
            switch (NumMet)
            {
                case 0: RKfunc = new Func<double, Vector, double, Vector>(RK4_1); eps = 0.0001; k = 4; break;
                case 1: RKfunc = new Func<double, Vector, double, Vector>(RK4_2); eps = 0.0001; k = 4; break;
                case 2: RKfunc = new Func<double, Vector, double, Vector>(RK3_1); eps = 0.001; k = 3; break;
                case 3: RKfunc = new Func<double, Vector, double, Vector>(RK3_2); eps = 0.001; k = 3; break;
                case 4: RKfunc = new Func<double, Vector, double, Vector>(RK2_1); eps = 0.01; k = 2; break;
                case 5: RKfunc = new Func<double, Vector, double, Vector>(RK2_2); eps = 0.01; k = 2; break;
            }
            t.Clear();
            z.Clear();

            int i = 0;
            double h;
            h = h0;
            Vector ynexth, ynexth2;
            double epsh, epsh2;
            double delta;
            t.Add(this.t0);
            z.Add(Y0);

            do
            {
                h = 2 * h;
                do
                {
                    h = h / 2;
                    ynexth = RKfunc(t[i], z[i], h);
                    ynexth2 = RKfunc(t[i], z[i], h / 2);
                    ynexth2 = RKfunc(t[i] + h / 2, ynexth2, h / 2);
                    double pow2k = System.Math.Pow(2, k);
                    epsh = ((ynexth2[0] - ynexth[0]) * pow2k) / (pow2k - 1);
                    epsh2 = (ynexth2[0] - ynexth[0]) / (pow2k - 1);
                } while (System.Math.Abs(epsh2) > eps);

                t.Add(t[i] + h);
                z.Add(ynexth2 + epsh2);
                i++;
                if (System.Math.Abs(epsh) <= eps)
                    h = h * 2;
                delta = t1 - t[i];
                if (h > delta)
                    h = delta;
            } while (delta > eps1);
        }
    }
}
