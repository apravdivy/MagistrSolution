using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TwoLayerDemo
{
    public partial class MainForm : Form
    {
        private double c1;
        private double c2;
        private double k;
        private double l;

        private double lambda1;

        private double lambda2;
        private int nodesCount;
        private double ro1;
        private double ro2;

        private int sensorsCount;
        private Dictionary<double, List<double>> sensorsResults;
        private double t0;
        private double tl;
        private double tr;


        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDo_Click(object sender, EventArgs e)
        {
            try
            {
                chart.Series.Clear();

                k = Convert.ToDouble(txtK.Text);
                l = Convert.ToDouble(txtL.Text);

                nodesCount = Convert.ToInt32(txtN.Text);

                lambda1 = Convert.ToDouble(txtLambda1.Text);
                ro1 = Convert.ToDouble(txtRo1.Text);
                c1 = Convert.ToDouble(txtC1.Text);

                lambda2 = Convert.ToDouble(txtLambda2.Text);
                ro2 = Convert.ToDouble(txtRo2.Text);
                c2 = Convert.ToDouble(txtC2.Text);

                tl = Convert.ToDouble(txtT1.Text);
                tr = Convert.ToDouble(txtTr.Text);
                t0 = Convert.ToDouble(txtT0.Text);

                sensorsCount = Convert.ToInt32(txtM.Text);

                sensorsResults = GetValue(lambda1, ro1, c1, lambda2, ro2, c2, tl, t0, tr, l, k, nodesCount, sensorsCount,
                                          true);

                double result = Solver.SolveMin(0, l, TragetFunctional, 0.0001);
                MessageBox.Show(result.ToString("f3"));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private double TragetFunctional(double ksi)
        {
            Dictionary<double, List<double>> calaculatedSensorsResults = GetValue(lambda1, ro1, c1, lambda2, ro2, c2, tl,
                                                                                  t0, tr, l, ksi, nodesCount,
                                                                                  sensorsCount, false);
            return Diff(calaculatedSensorsResults, sensorsResults);
        }

        private double Diff(Dictionary<double, List<double>> calaculatedSensorsResults,
                            Dictionary<double, List<double>> sensorsRes)
        {
            double res = sensorsRes.Sum(sensorsResult => ListDiff(sensorsResult.Value, calaculatedSensorsResults[sensorsResult.Key]));
            return res/2.0;
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
            var n1 = (int) (k*nodesCount/l);
            int n2 = nodesCount - n1;
            double d = l/(sensorsCount + 1);
            double h = l/(nodesCount - 1);
            int step = 1;
            if (d > h)
            {
                step = (int) (d/h) + 1;
            }

            var sensorsResults = new Dictionary<double, List<double>>();

            int i = 0;

            for (int t = 1; t <= 600; t += 10)
            {
                double[] T = Solver.SolveTwoLayer(n1, n2, t, l, lambda1, ro1, c1, lambda2, ro2, c2, tl, t0, tr, h);
                if (isDraw)
                {
                    AddSeries(i, h, t, T);
                }
                int currentPos = 0;
                for (int z = 0; z < T.Length; z++)
                {
                    if (currentPos == step)
                    {
                        if (!sensorsResults.ContainsKey(z*h))
                        {
                            sensorsResults.Add(z*h, new List<double>());
                        }
                        sensorsResults[z*h].Add(T[z]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            double res = Solver.SolveMin(-3, 5, x => x*x);
            MessageBox.Show(res.ToString("f3", CultureInfo.InvariantCulture));
        }
    }
}