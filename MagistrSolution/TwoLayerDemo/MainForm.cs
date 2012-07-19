using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
                double L, lambda1, lambda2, ro1, ro2, c1, c2, Tl, T0, Tr;

                N1 = Convert.ToInt32(txtN1.Text);
                N2 = Convert.ToInt32(txtN2.Text);

                L = Convert.ToDouble(txtL.Text);

                lambda1 = Convert.ToDouble(txtLambda1.Text);
                ro1 = Convert.ToDouble(txtRo1.Text);
                c1 = Convert.ToDouble(txtC1.Text);

                lambda2 = Convert.ToDouble(txtLambda2.Text);
                ro2 = Convert.ToDouble(txtRo2.Text);
                c2 = Convert.ToDouble(txtC2.Text);

                Tl = Convert.ToDouble(txtT1.Text);
                Tr = Convert.ToDouble(txtTr.Text);
                T0 = Convert.ToDouble(txtT0.Text);


                var t_ends = new List<double>() { 30, 180, 600 }; //Convert.ToDouble(txtT_end.Text);
                int i = 0;
                chart.Series.Clear();
                foreach (var t_end in t_ends)
                {
                    double h;
                    double[] T;

                    Solver.SolveTwoLayer(N1, N2, t_end, L, lambda1, lambda2, ro1, ro2, c1, c2, Tl, T0, Tr, out h, out T);

                    chart.Series.Add(string.Format("Result_{0}", t_end));
                    chart.Series[i].ChartType = SeriesChartType.Line;

                    double x = 0;
                    for (int j = 0; j < T.Length; j++)
                    {
                        chart.Series[i].Points.AddXY(x, T[j]);
                        x += h;
                    }
                    i++;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }


    }
}


