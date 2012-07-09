using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TwoLayerDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            try
            {
                int N1, N2;
                double t_end, L, lambda1, lambda2, ro1, ro2, c1, c2, T1, T0, Tr;

                N1 = Convert.ToInt32(txtN1.Text);
                N2 = Convert.ToInt32(txtN2.Text);

                t_end = Convert.ToDouble(txtT_end.Text);
                L = Convert.ToDouble(txtL.Text);
                lambda1 = Convert.ToDouble(txtLambda1.Text);
                lambda2 = Convert.ToDouble(txtLambda2.Text);
                ro1 = Convert.ToDouble(txtRo1.Text);
                ro2 = Convert.ToDouble(txtRo2.Text);
                c1 = Convert.ToDouble(txtC1.Text);
                c2 = Convert.ToDouble(txtC2.Text);

                T1 = Convert.ToDouble(txtT1.Text);
                Tr = Convert.ToDouble(txtTr.Text);
                T0 = Convert.ToDouble(txtT0.Text);

                int N = N1 + N2 + 1;
                double h = L / (N - 1);
                double a1 = lambda1 / (ro1 * c1);
                double a2 = lambda2 / (ro2 * c2);
                double tau = t_end / 100;

                double[] T = new double[N];
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
                    beta[0] = T1;

                    for (int i = 1; i < N1; i++)
                    {
                        double ai, bi, ci, fi;
                        ai = ci = lambda1 / Math.Pow(h, 2);
                        bi = 2 * ai + ro1 * c1 / tau;
                        fi = ro1 * ci * T[i] / tau;

                        alpha[i] = ai / (bi - ci * alpha[i - 1]);
                        beta[i] = (ci - beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                    }
                    alpha[N1] = 2 * a1 * a2 * tau * lambda2 / (lambda2 + lambda1 * (1 - alpha[N1 - 1])) + h * h * (a1 * lambda2 + a2 * lambda1);
                    beta[N1] = (2 * a1 * a2 * tau * lambda1 * beta[N1 - 1] + h * h * (a1 * lambda2 + a2 * lambda1) * T[N1]) / (2 * a1 * a2 * tau * (lambda2 + lambda1 * (1 - alpha[N1 - 1])) + h * h * (a1 * lambda2 + a2 * lambda1));

                    for (int i = N1 + 1; i < N; i++)
                    {
                        double ai, bi, ci, fi;
                        ai = lambda2 / (h * h);
                        bi = a2 * lambda2 / (h * h) + ro2 * c2 / tau;
                        ci = lambda2 / (h * h);
                        fi = ro2 * c2 * T[i] / tau;

                        alpha[i] = ai / (bi - ci * alpha[i - 1]);
                        beta[i] = (ci * beta[i - 1] - fi) / (bi - ci * alpha[i - 1]);
                    }

                    T[N - 1] = Tr;

                    for (int i = N - 2; i >= 0; i--)
                    {
                        T[i] = alpha[i] * T[i + 1] + beta[i];
                    }


                }

                chart1.Series.Clear();
                chart1.Series.Add("Result");
                chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                double x = 0;
                for (int i = 0; i < T.Length; i++)
                {
                    chart1.Series[0].Points.AddXY(x, T[i]);
                    x += h;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }


    }
}


