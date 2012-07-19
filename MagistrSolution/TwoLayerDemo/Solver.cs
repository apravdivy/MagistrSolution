using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoLayerDemo
{
    static class Solver
    {
        public static void SolveTwoLayer(int N1, int N2, double t_end, double L, double lambda1, double lambda2, double ro1, double ro2, double c1, double c2, double Tl, double T0, double Tr, out double h, out double[] T)
        {
            int N = N1 + N2 + 1;
            h = L / (N - 1);
            double a1 = lambda1 / (ro1 * c1);
            double a2 = lambda2 / (ro2 * c2);
            double tau = t_end / 100;

            T = new double[N];
            for (int i = 0; i < N; i++)
            {
                T[i] = T0;
            }
            double time = 0;

            double[] alpha = new double[N];
            double[] beta = new double[N];
            while (time < t_end)
            {
                time += tau;
                alpha[0] = 0;
                beta[0] = Tl;

                for (int i = 1; i < N1; i++)
                {
                    double ai, bi, ci, fi;
                    ai = lambda1 / (h * h);
                    bi = 2 * lambda1 / (h * h) + ro1 * c1 / tau;
                    ci = lambda1 / (h * h);
                    fi = -ro1 * c1 * T[i] / tau;

                    alpha[i] = ai / (bi - ci * alpha[i - 1]);
                    beta[i] = (ci * beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                }
                alpha[N1] = 2.0 * a1 * a2 * tau * lambda2 / (2 * a1 * a2 * tau * (lambda2 + lambda1 * (1 - alpha[N1 - 1])) + h * h * (a1 * lambda2 + a2 * lambda1));
                beta[N1] = (2.0 * a1 * a2 * tau * lambda1 * beta[N1 - 1] + (h * h) * (a1 * lambda2 + a2 * lambda1) * T[N1]) / (2 * a1 * a2 * tau * (lambda2 + lambda1 * (1 - alpha[N1 - 1])) + (h * h) * (a1 * lambda2 + a2 * lambda1));

                for (int i = N1 + 1; i < N - 1; i++)
                {
                    double ai, bi, ci, fi;
                    ai = lambda2 / (h * h);
                    bi = 2.0 * lambda2 / (h * h) + ro2 * c2 / tau;
                    ci = lambda2 / (h * h);
                    fi = -ro2 * c2 * T[i] / tau;

                    alpha[i] = ai / (bi - ci * alpha[i - 1]);
                    beta[i] = (ci * beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                }

                T[N - 1] = Tr;

                for (int i = N - 2; i >= 0; i--)
                {
                    T[i] = alpha[i] * T[i + 1] + beta[i];
                }


            }
        }
    }
}
