using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Core;
using ZedGraph;

namespace View.Forms
{
    public partial class ResultForm : Form
    {
        public ResultForm(MainForm form,
                          double[,] sp, double[,] rp1, double[,] rp2, int n, int m,
                          string inf1, string inf2, List<List<double>> spar)
        {
            InitializeComponent();

            if (form.setCount == 1)
            {
                form.view.ConfigGraphPane(zgc, this, PaneLayout.SingleColumn, new List<ChartInfo>
                                                                                  {
                                                                                      new ChartInfo
                                                                                          {
                                                                                              Title = "(t,z1)",
                                                                                              XAxisTitle = "t",
                                                                                              YAxisTitle = "z1"
                                                                                          }
                                                                                  });
            }
            else
            {
                form.view.ConfigGraphPane(zgc, this, PaneLayout.SingleColumn, new List<ChartInfo>
                                                                                  {
                                                                                      new ChartInfo
                                                                                          {
                                                                                              Title = "(t,z1)",
                                                                                              XAxisTitle = "t",
                                                                                              YAxisTitle = "z1"
                                                                                          },
                                                                                      new ChartInfo
                                                                                          {
                                                                                              Title = "(z1,z2)",
                                                                                              XAxisTitle = "z1",
                                                                                              YAxisTitle = "z2"
                                                                                          }
                                                                                  });
            }
            AddGraph(form, form.curveName, form.curveNameSuff, Color.Black, SymbolType.None);
            AddGraph(form, form.curveName1, form.curveNameSuff, Color.Green, SymbolType.Circle);
            AddGraph(form, form.curveName2, form.curveNameSuff, Color.Blue, SymbolType.Triangle);

            zgc.AxisChange();
            zgc.Refresh();

            dgvStartPar.RowCount = dgvResPar1.RowCount = dgvResPar2.RowCount = n;
            dgvStartPar.ColumnCount = dgvResPar1.ColumnCount = dgvResPar2.ColumnCount = m;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    dgvStartPar.Rows[i].Cells[j].Value = string.Format("{0:f6}", sp[i, j]);
                    dgvResPar1.Rows[i].Cells[j].Value = string.Format("{0:f6}", rp1[i, j]);
                    dgvResPar2.Rows[i].Cells[j].Value = string.Format("{0:f6}", rp2[i, j]);
                }
            }

            var s = new StringBuilder();
            int a = 0;
            int b = 0;
            string line;
            s.AppendLine("Start parameters for RAlg:");
            s.AppendLine("----");
            foreach (var row in spar)
            {
                line = string.Format("{0})", a);
                foreach (double val in row)
                {
                    line += string.Format("a{0}={1:f6} ", b, val);
                    b++;
                }
                s.AppendLine(line);
                a++;
            }
            s.Append("----");
            memoEdit1.Text = s.ToString();

            label1.Text = string.IsNullOrEmpty(inf1) ? "--" : inf1;
            label2.Text = string.IsNullOrEmpty(inf2) ? "--" : inf2;
        }

        private void AddGraph(MainForm form, string s, string sf, Color c, SymbolType t)
        {
            if (form.curves.ContainsKey(s) && form.curves[s].Count > 0)
            {
                if (form.setCount == 2)
                {
                    zgc.MasterPane[1].AddCurve(s, form.curves[s], c, t);
                    zgc.MasterPane[0].AddCurve(s + sf, form.curves[s + sf], c, t);
                }
                else
                {
                    zgc.MasterPane[0].AddCurve(s, form.curves[s], c, t);
                }
            }
        }
    }
}