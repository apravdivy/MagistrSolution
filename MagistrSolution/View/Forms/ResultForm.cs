using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                form.view.ConfigGraphPane(this.zgc, this, PaneLayout.SingleColumn, new List<Core.ChartInfo>()
                {
                    new Core.ChartInfo(){ Title="(t,z1)", XAxisTitle="t",YAxisTitle="z1"}
                });

            }
            else
            {
                form.view.ConfigGraphPane(this.zgc, this, PaneLayout.SingleColumn, new List<Core.ChartInfo>()
                {
                    new Core.ChartInfo(){ Title="(t,z1)", XAxisTitle="t",YAxisTitle="z1"},
                      new Core.ChartInfo(){ Title="(z1,z2)", XAxisTitle="z1",YAxisTitle="z2"}
                 
                });
            }
            this.AddGraph(form, form.curveName, form.curveNameSuff, Color.Black, SymbolType.None);
            this.AddGraph(form, form.curveName1, form.curveNameSuff, Color.Green, SymbolType.Circle);
            this.AddGraph(form, form.curveName2, form.curveNameSuff, Color.Blue, SymbolType.Triangle);

            this.zgc.AxisChange();
            this.zgc.Refresh();

            this.dgvStartPar.RowCount = this.dgvResPar1.RowCount = this.dgvResPar2.RowCount = n;
            this.dgvStartPar.ColumnCount = this.dgvResPar1.ColumnCount = this.dgvResPar2.ColumnCount = m;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    this.dgvStartPar.Rows[i].Cells[j].Value = string.Format("{0:f6}", sp[i, j]);
                    this.dgvResPar1.Rows[i].Cells[j].Value = string.Format("{0:f6}", rp1[i, j]);
                    this.dgvResPar2.Rows[i].Cells[j].Value = string.Format("{0:f6}", rp2[i, j]);
                }
            }

            StringBuilder s = new StringBuilder();
            int a = 0;
            int b = 0;
            string line;
            s.AppendLine("Start parameters for RAlg:");
            s.AppendLine("----");
            foreach (List<double> row in spar)
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
            this.memoEdit1.Text = s.ToString();

            this.label1.Text = string.IsNullOrEmpty(inf1) ? "--" : inf1;
            this.label2.Text = string.IsNullOrEmpty(inf2) ? "--" : inf2;
        }

        private void AddGraph(MainForm form, string s, string sf, Color c, SymbolType t)
        {
            if (form.curves.ContainsKey(s) && form.curves[s].Count > 0)
            {
                if (form.setCount == 2)
                {
                    this.zgc.MasterPane[1].AddCurve(s, form.curves[s], c, t);
                    this.zgc.MasterPane[0].AddCurve(s + sf, form.curves[s + sf], c, t);
                }
                else
                {
                    this.zgc.MasterPane[0].AddCurve(s, form.curves[s], c, t);
                }
            }
        }

    }
}

