using System;

namespace TwoLayerDemo
{
    internal static class Solver
    {
        public static double[] SolveTwoLayer(int n1, int n2, double tEnd, double lambda1, double ro1,
                                             double c1, double lambda2, double ro2, double c2, double tl, double t0,
                                             double tr, double h)
        {
            int n = n1 + n2 + 1;

            double a1 = lambda1 / (ro1 * c1);
            double a2 = lambda2 / (ro2 * c2);
            double tau = tEnd / 100;

            var T = new double[n];
            for (int i = 0; i < n; i++)
            {
                T[i] = t0;
            }
            double time = 0;

            var alpha = new double[n];
            var beta = new double[n];
            while (time < tEnd)
            {
                time += tau;
                alpha[0] = 0;
                beta[0] = tl;

                for (int i = 1; i < n1; i++)
                {
                    double ai = lambda1 / (h * h);
                    double bi = 2 * lambda1 / (h * h) + ro1 * c1 / tau;
                    double ci = lambda1 / (h * h);
                    double fi = -ro1 * c1 * T[i] / tau;

                    alpha[i] = ai / (bi - ci * alpha[i - 1]);
                    beta[i] = (ci * beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                }
                alpha[n1] = 2.0 * a1 * a2 * tau * lambda2 /
                            (2 * a1 * a2 * tau * (lambda2 + lambda1 * (1 - alpha[n1 - 1])) + h * h * (a1 * lambda2 + a2 * lambda1));
                beta[n1] = (2.0 * a1 * a2 * tau * lambda1 * beta[n1 - 1] + (h * h) * (a1 * lambda2 + a2 * lambda1) * T[n1]) /
                           (2 * a1 * a2 * tau * (lambda2 + lambda1 * (1 - alpha[n1 - 1])) + (h * h) * (a1 * lambda2 + a2 * lambda1));

                for (int i = n1 + 1; i < n - 1; i++)
                {
                    double ai = lambda2 / (h * h);
                    double bi = 2.0 * lambda2 / (h * h) + ro2 * c2 / tau;
                    double ci = lambda2 / (h * h);
                    double fi = -ro2 * c2 * T[i] / tau;

                    alpha[i] = ai / (bi - ci * alpha[i - 1]);
                    beta[i] = (ci * beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                }

                T[n - 1] = tr;

                for (int i = n - 2; i >= 0; i--)
                {
                    T[i] = alpha[i] * T[i + 1] + beta[i];
                }
            }
            return T;
        }

        public static Tuple<double, double> SolveMin(double a, double b, Func<double, double> f, double? eps = null)
        {
            if (eps == null)
            {
                eps = 0.001;
            }
            double fi = (1 + Math.Pow(5, 0.5)) / 2.0;

            double x1 = b - (b - a) / fi;
            double x2 = a + (b - a) / fi;

            double y1 = f(x1);
            double y2 = f(x2);

            while (Math.Abs(b - a) > eps)
            {
                if (y1 >= y2)
                {
                    a = x1;
                    x1 = x2;
                    y1 = y2;
                    x2 = b - (x1 - a);
                    y2 = f(x2);
                }
                else
                {
                    b = x2;
                    x2 = x1;
                    y2 = y1;
                    x1 = a + (b - x2);
                    y1 = f(x1);
                }
            }
            return new Tuple<double, double>((a + b) / 2.0, (y1 + y2) / 2.0);
        }
    }
}