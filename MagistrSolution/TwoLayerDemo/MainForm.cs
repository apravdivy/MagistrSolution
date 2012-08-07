using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Common.Mathematic;
using System.Globalization;
using TwoLayerDemo.Properties;

namespace TwoLayerDemo
{
    public partial class MainForm : Form
    {
        private double _c1;
        private double _c2;
        private double _k;
        private double _l;

        private double _lambda1;

        private double _lambda2;
        private int _nodesCount;
        private double _ro1;
        private double _ro2;

        private int _sensorsCount;
        private Dictionary<double, List<double>> _sensorsResults;
        private double _t0;
        private double _tl;
        private double _tr;
        private double _delta;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            try
            {
                var cs = new CancellationTokenSource();
                var t = new Task<double>(() =>
                {
                    GetValuesFromForm();
                    _sensorsResults = GetValue(_lambda1, _ro1, _c1, _lambda2, _ro2, _c2, _tl, _t0, _tr, _l, _k, _nodesCount,
                                              _sensorsCount,
                                              true);
                    double result = Solver.SolveMin(0, _l, TargetFunctional, 0.0001);
                    return result;
                }, cs.Token);

                t.ContinueWith(task =>
                {
                    MessageBox.Show(task.Result.ToString("f3"));
                }, cs.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

                t.ContinueWith(task =>
                {
                    MessageBox.Show(Resources.ErrorString);
                }, cs.Token, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                t.Start(TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void GetValuesFromForm()
        {
            chart.Series.Clear();

            _k = Convert.ToDouble(txtK.Text);
            _l = Convert.ToDouble(txtL.Text);

            _nodesCount = Convert.ToInt32(txtN.Text);

            _lambda1 = Convert.ToDouble(txtLambda1.Text);
            _ro1 = Convert.ToDouble(txtRo1.Text);
            _c1 = Convert.ToDouble(txtC1.Text);

            _lambda2 = Convert.ToDouble(txtLambda2.Text);
            _ro2 = Convert.ToDouble(txtRo2.Text);
            _c2 = Convert.ToDouble(txtC2.Text);

            _tl = Convert.ToDouble(txtT1.Text);
            _tr = Convert.ToDouble(txtTr.Text);
            _t0 = Convert.ToDouble(txtT0.Text);
            _delta = Convert.ToDouble(txtDelta.Text, CultureInfo.CurrentUICulture);
            _sensorsCount = Convert.ToInt32(txtM.Text);
        }

        private double TargetFunctional(double ksi)
        {
            Dictionary<double, List<double>> calaculatedSensorsResults = GetValue(_lambda1, _ro1, _c1, _lambda2, _ro2, _c2,
                                                                                  _tl,
                                                                                  _t0, _tr, _l, ksi, _nodesCount,
                                                                                  _sensorsCount, false);
            return Diff(calaculatedSensorsResults, _sensorsResults);
        }

        private double TargetFunctional(double ksi, double l1, double l2)
        {
            Dictionary<double, List<double>> calaculatedSensorsResults = GetValue(l1, _ro1, _c1, l2, _ro2, _c2, _tl,
                                                                                  _t0, _tr, _l, ksi, _nodesCount,
                                                                                  _sensorsCount, false);
            return Diff(calaculatedSensorsResults, _sensorsResults);
        }

        private double Diff(Dictionary<double, List<double>> calaculatedSensorsResults,
                            Dictionary<double, List<double>> sensorsRes)
        {
            double res =
                sensorsRes.Sum(
                    sensorsResult => ListDiff(sensorsResult.Value, calaculatedSensorsResults[sensorsResult.Key]));
            return res / 2.0;
        }

        private double ListDiff(List<double> list1, List<double> list2)
        {
            double r = 0;
            for (int i = 0; i < list1.Count; i++)
            {
                r += Math.Pow((list1[i] - list2[i]), 2);
            }
            return r;
        }

        private Dictionary<double, List<double>> GetValue(double lambda1, double ro1, double c1, double lambda2,
                                                          double ro2, double c2, double tl, double t0, double tr,
                                                          double l, double k, int nodesCount, int sensorsCount,
                                                          bool isDraw)
        {
            Debug.Assert(k > 0 && k < l);
            var n1 = (int)(k * nodesCount / l);
            int n2 = nodesCount - n1;
            Debug.Assert(n2 >= 0);
            double d = l / (sensorsCount + 1);
            double h = l / (nodesCount - 1);
            int step = 1;
            if (d > h)
            {
                step = (int)(d / h) + 1;
            }

            var sensorsResults = new Dictionary<double, List<double>>();

            int i = 0;
            Random r = new Random();
            for (int t = 1; t <= 600; t += 10)
            {
                double[] T = Solver.SolveTwoLayer(n1, n2, t, l, lambda1, ro1, c1, lambda2, ro2, c2, tl, t0, tr, h);

                for (int index = 0; index < T.Length; index++)
                {
                    var dd = r.NextDouble() > 0.5 ? r.NextDouble() * _delta : -r.NextDouble() * _delta;
                    //System.Diagnostics.Debug.WriteLine(dd);
                    T[index] += dd;
                }
                if (isDraw)
                {
                    AddSeries(i, h, t, T);
                }
                int currentPos = 0;
                for (int z = 0; z < T.Length; z++)
                {
                    if (currentPos == step)
                    {
                        if (!sensorsResults.ContainsKey(z * h))
                        {
                            sensorsResults.Add(z * h, new List<double>());
                        }
                        sensorsResults[z * h].Add(T[z]);
                        currentPos = 0;
                    }
                    else
                    {
                        currentPos++;
                    }
                }
                i++;
            }
            if (isDraw)
            {
                int dNumber = 1;
                foreach (var p in sensorsResults)
                {
                    var s = new Series(string.Format("Sensor_{0}", dNumber))
                                {
                                    ChartType = SeriesChartType.Line,
                                    BorderWidth = 3,
                                    BorderDashStyle = ChartDashStyle.Dash,
                                    BorderColor = Color.Black
                                };
                    foreach (double y in p.Value)
                    {
                        s.Points.AddXY(p.Key, y);
                    }
                    dNumber++;
                    chart.Series.Add(s);
                }
            }
            return sensorsResults;
        }

        private void AddSeries(int i, double h, int t, double[] T)
        {
            chart.Series.Add(string.Format("Result_{0}", t));
            chart.Series[i].ChartType = SeriesChartType.Line;

            double x = 0;
            foreach (double t1 in T)
            {
                chart.Series[i].Points.AddXY(x, t1);
                x += h;
            }
        }

        private void RAlgFunction(ref double f, ref double[] x, ref double[] g)
        {
            int a = 1000000;
            f = TargetFunctional(x[0], x[1], x[2]);
            var prevF = f;

            //---------- Штрафы -------------
            if (x[0] < 0)
            {
                f += a * x[0] * x[0];
            }
            if (x[0] > _l)
            {
                f += a * (x[0] - _l) * (x[0] - _l);
            }
            double delta = 0.001;

            g[0] = (TargetFunctional(x[0] + delta, x[1], x[2]) - prevF) / delta;
            g[1] = (TargetFunctional(x[0], x[1] + 1, x[2]) - prevF);
            g[2] = (TargetFunctional(x[0], x[1], x[2] + 1) - prevF);

            if (x[0] < 0)
            {
                g[0] += 2 * a * x[0];
            }
            if (x[0] > _l)
            {
                g[0] += 2 * a * (x[0] - _l);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

            var cs = new CancellationTokenSource();
            var t = new Task<RAlgSolver>(() =>
            {
                GetValuesFromForm();
                _sensorsResults = GetValue(_lambda1, _ro1, _c1, _lambda2, _ro2, _c2, _tl, _t0, _tr, _l, _k, _nodesCount,
                                              _sensorsCount,
                                              true);

                var ra = new RAlgSolver();
                ra.FUNCT = RAlgFunction;
                ra.R_Algorithm();
                return ra;
            }, cs.Token);

            t.ContinueWith(task =>
            {
                MessageBox.Show(string.Format("{0} {1} {2}", task.Result.x[0], task.Result.x[1], task.Result.x[2]));
            }, cs.Token, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            t.ContinueWith(task =>
            {
                MessageBox.Show(Resources.ErrorString);
            }, cs.Token, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            t.Start(TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var ra = new RAlgSolver(2);
            ra.FUNCT = testFunction;
            ra.R_Algorithm();
            MessageBox.Show(string.Format("x1: {0:f3} x2: {1:f3}", ra.x[0], ra.x[1]));

        }

        private void testFunction(ref double f_1, ref double[] x_1, ref double[] g_1)
        {
            int a = 10000000;
            f_1 = x_1[0] * x_1[0] + x_1[1] * x_1[1];

            if (x_1[0] < 1)
            {
                f_1 += a * (1 - x_1[0]) * (1 - x_1[0]);
            }

            g_1[0] = 2 * x_1[0];
            g_1[1] = 2 * x_1[1];

            if (x_1[0] < 1) g_1[0] += -2 * a * (1 - x_1[0]);
        }
    }
}