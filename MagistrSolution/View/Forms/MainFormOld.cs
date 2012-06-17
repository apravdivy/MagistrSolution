using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common;
using ZedGraph;
using System.Threading;
using System.Configuration;
using System.IO;
using Common.Mathematic;
using System.Reflection;
using DevExpress.XtraEditors.Controls;

namespace View.Forms
{
    public partial class MainFormOld : Form
    {
        private delegate double[] EmptyDelegate();

        private View view;

        private Dictionary<string, PointPairList> curves = new Dictionary<string, PointPairList>();
        private Dictionary<string, PointPairList> curves1 = new Dictionary<string, PointPairList>();
        private Dictionary<string, PointPairList> curves2 = new Dictionary<string, PointPairList>();

        private Dictionary<string, List<ResPointViewType2>> r = null;

        public MainFormOld(View view)
        {
            InitializeComponent();
            this.view = view;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.view.ViewActionCall(ViewEventType.Exit);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.view.ViewActionCall(ViewEventType.Exit);
        }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            this.btnUpdate_Click(null, null);
            this.view.ConfigGraphPane(this.zgc1, this, PaneLayout.SingleRow, new List<Core.ChartInfo>(){
                new Core.ChartInfo()
                { Title= "Depends t with z",XAxisTitle= "t",YAxisTitle= "z"}});
            ZedGraph.GraphPane gp = this.zgc1.MasterPane[0];
            gp.CurveList.Clear();

            string fname = listFunctions1.SelectedItem.ToString();
            int randn = int.Parse(txtRandomCount.Text);
            this.curves.Clear();
            Random r = new Random();
            for (int i = 0; i < randn; i++)
            {
                string curveName = string.Format("curve{0}", i);
                this.curves.Add(curveName, new PointPairList());
                Color c = Color.Black;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
                gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
                this.view.ViewActionCall(ViewEventType.StartSolving1, fname, curveName);
            }
        }

        private void btnGo2_Click(object sender, EventArgs e)
        {
            this.btnUpdate_Click(null, null);

            List<Core.ChartInfo> chInfo = new List<Core.ChartInfo>();
            chInfo.Add(new Core.ChartInfo() { Title = "Depends (t,z1)", XAxisTitle = "t", YAxisTitle = "z1" });
            chInfo.Add(new Core.ChartInfo() { Title = "Depends (t,z1)", XAxisTitle = "t", YAxisTitle = "z2" });
            chInfo.Add(new Core.ChartInfo() { Title = "Depends (z1,z2)", XAxisTitle = "z1", YAxisTitle = "z2" });

            this.view.ConfigGraphPane(this.zgc1, this, PaneLayout.ExplicitCol21, chInfo);

            ZedGraph.GraphPane gp = this.zgc1.MasterPane[0];
            ZedGraph.GraphPane gp1 = this.zgc1.MasterPane[1];
            ZedGraph.GraphPane gp2 = this.zgc1.MasterPane[2];
            gp.CurveList.Clear();
            gp1.CurveList.Clear();
            gp2.CurveList.Clear();

            string fname = listFunctions2.SelectedItem.ToString();
            int randn = int.Parse(txtRandomCount.Text);
            this.curves.Clear();
            this.curves1.Clear();
            this.curves2.Clear();
            Random r = new Random();
            for (int i = 0; i < randn; i++)
            {
                string curveName = string.Format("curve{0}", i);

                this.curves.Add(curveName, new PointPairList());
                this.curves1.Add(curveName, new PointPairList());
                this.curves2.Add(curveName, new PointPairList());

                Color c = Color.Black;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));

                gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
                gp1.AddCurve(curveName, this.curves1[curveName], c, SymbolType.None);
                gp2.AddCurve(curveName, this.curves2[curveName], c, SymbolType.None);

                this.view.ViewActionCall(ViewEventType.StartSolving2, fname, curveName);
            }


            //this.tabControl.SelectedIndex = 0;
        }

        private void RefreshaAllCharts(int n)
        {
            zgc1.AxisChange();
            zgc1.Refresh();
            //if (n == 2)
            //{
            //    this.zgc2.AxisChange();
            //    this.zgc2.Refresh();
            //    this.zgc3.AxisChange();
            //    this.zgc3.Refresh();
            //}
        }

        public void DrawPoint(double x, List<double> val, string curveName)
        {
            this.curves[curveName].Add(x, val[0]);
            this.RefreshaAllCharts(1);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            listFunctions1.Items.Clear();
            listFunctions2.Items.Clear();

            Functions.GetFunctionNames(1).ForEach(x => listFunctions1.Items.Add(x));
            Functions.GetFunctionNames(2).ForEach(x => listFunctions2.Items.Add(x));

            if (listFunctions1.Items.Count > 0)
            {
                listFunctions1.SelectedIndex = 0;
            }
            else
            {
                this.btnGo1.Enabled = false;
            }
            if (listFunctions2.Items.Count > 0)
            {
                listFunctions2.SelectedIndex = 0;
            }
            else
            {
                this.btnGo2.Enabled = false;
            }

            this.view.UpdateAlgorithmParameters(this.txtT0.Text, this.txtT1.Text, this.txtY0.Text, this.txtY00.Text, txtY01.Text, this.txtN.Text, this.txtRandomCount.Text, this.txtz1min.Text, this.txtz1max.Text, this.txtz2min.Text, this.txtz2max.Text);

            //this.view.ConfigGraphPane(this.zgc1.GraphPane, "Depends z1 with t", "t", "z1");
            //this.view.ConfigGraphPane(this.zgc2.GraphPane, "Depends z2 with t", "t", "z2");
            //this.view.ConfigGraphPane(this.zgc3.GraphPane, "Depends z1 with z2", "z1", "z2");

            this.view.ConfigGraphPane(this.zgSolve1, this, PaneLayout.SingleRow, new List<Core.ChartInfo>(){
                new Core.ChartInfo()
                { Title= "Depends t with z",XAxisTitle= "t",YAxisTitle= "z"}});

            Type type = typeof(Functions_2_2);

            MethodInfo[] methods = type.GetMethods();
            fnames.Clear();
            foreach (MethodInfo m in methods)
            {
                object[] atr = m.GetCustomAttributes(typeof(FAtrAttribute), false);
                if (atr != null && atr.Length > 0)
                {
                    if ((atr[0] as FAtrAttribute).funcType == FuncType.Second)
                    {
                        sfList.Items.Add((atr[0] as FAtrAttribute).funcName);
                        this.pCountList.Add((atr[0] as FAtrAttribute).funcName.ToUpper(), (atr[0] as FAtrAttribute).PCount);
                        fnames.Add((atr[0] as FAtrAttribute).funcName.ToUpper(), m.Name);
                    }
                }
            }

            mfList.Items.Add("MainFunction_2Set");
            mfList.Items.Add("MainFunction_3Set");
            mfList.Items.Add("MainFunction_4Set");
            mfList.Items.Add("MainFunction_5Set");

            sfList.SelectedIndex = 0;
            mfList.SelectedIndex = 0;

            int i = 0;
            foreach (Task t in this.view.taskCollection)
            {
                this.lstTasks.Items.Add(i);
                i++;
            }
        }
        private Dictionary<string, string> fnames = new Dictionary<string, string>();

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.view.UpdateAlgorithmParameters(this.txtT0.Text, this.txtT1.Text, this.txtY0.Text, this.txtY00.Text, txtY01.Text, this.txtN.Text, this.txtRandomCount.Text, this.txtz1min.Text, this.txtz1max.Text, this.txtz2min.Text, this.txtz2max.Text);
        }

        internal void DrawPoint2(double p, List<double> list, string curveName)
        {
            this.curves[curveName].Add(p, list[0]);
            this.curves1[curveName].Add(p, list[1]);
            this.curves2[curveName].Add(list[0], list[1]);
            this.RefreshaAllCharts(2);
        }


        private void btnSolvePodhod2_Click(object sender, EventArgs e)
        {
            this.btnUpdate_Click(null, null);
            this.listBox1.Items.Clear();


            ZedGraph.GraphPane gp = this.zgSolve1.GraphPane;
            gp.CurveList.Clear();
            this.curves.Clear();

            string fname = listFunctions1.SelectedItem.ToString();

            string curveName = "Line";
            this.curves.Add(curveName, new PointPairList());
            Color c = Color.Black;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
            this.view.ViewActionCall(ViewEventType.SolvePodhod2Type1, fname, curveName);
        }

        internal void ShowResultType1(List<double> divT, List<double> divZ, List<ResPointViewType1> list, double t0, double t)
        {
            this.dgv.DataSource = null;
            this.dgv.DataSource = list;

            if (divT.Count != divZ.Count)
                throw new ArgumentException("WTF?");

            for (int i = 0; i < divZ.Count; i++)
            {
                this.zgSolve1.GraphPane.AddCurve(i.ToString(), new PointPairList(), Color.Blue, SymbolType.None);
                this.zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(t0, divZ[i]);
                this.zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(t, divZ[i]);
            }

            this.zgSolve1.AxisChange();
            this.zgSolve1.Refresh();

            //MessageBox.Show(string.Format("t={0} : z={1}",divT,divZ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.btnUpdate_Click(null, null);
            this.listBox1.Items.Clear();


            ZedGraph.GraphPane gp = this.zgSolve1.GraphPane;
            gp.CurveList.Clear();
            this.curves.Clear();

            string fname = listFunctions2.SelectedItem.ToString();

            string curveName = "Line2";
            this.curves.Add(curveName, new PointPairList());
            Color c = Color.DarkBlue;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
            this.view.ViewActionCall(ViewEventType.SolovePodhod2Type2, fname, curveName);

        }

        internal void DrawPhasePortret(double p, List<double> list, string curveName)
        {
            this.curves[curveName].Add(list[0], list[1]);
            this.zgSolve1.AxisChange();
            this.zgSolve1.Refresh();
        }

        internal void ShowResultType2(List<double> divT, List<double> divZ, List<ResPointViewType2> list, double z2min, double z2max)
        {
            this.dgv.DataSource = null;
            this.dgv.DataSource = list;

            if (divT.Count != divZ.Count)
                throw new ArgumentException("WTF?");

            for (int i = 0; i < divZ.Count; i++)
            {
                this.zgSolve1.GraphPane.AddCurve(i.ToString(), new PointPairList(), Color.Blue, SymbolType.None);
                this.zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(divZ[i], z2min);
                this.zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(divZ[i], z2max);
            }

            this.zgSolve1.AxisChange();
            this.zgSolve1.Refresh();

        }

        private void btnSolvingType2Mass_Click(object sender, EventArgs e)
        {
            this.btnUpdate_Click(null, null);
            this.listBox1.Items.Clear();
            ZedGraph.GraphPane gp = this.zgSolve1.GraphPane;
            gp.CurveList.Clear();
            this.curves.Clear();
            string fname = listFunctions2.SelectedItem.ToString();

            int randonCount = int.Parse(this.txtRandomCount.Text);
            string[,] curveNames = new string[randonCount, randonCount];
            for (int i = 0; i < randonCount; i++)
            {
                for (int j = 0; j < randonCount; j++)
                {
                    string curveName = string.Format("Line-{0}_{1}", i, j);
                    this.curves.Add(curveName, new PointPairList());
                    Color c = Color.DarkBlue;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
                    gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
                    curveNames[i, j] = curveName;
                }
            }
            this.view.ViewActionCall(ViewEventType.SolovePodhod2Type2Mass, fname, curveNames);
        }

        internal void ShowResultType2Mass(ZedGraph.PointPairList p, Dictionary<string, List<ResPointViewType2>> r)
        {
            this.listBox1.Items.Clear();
            this.r = r;
            foreach (string s in r.Keys)
            {
                this.listBox1.Items.Add(s);
            }
            //p.Sort(SortType.XValues);
            this.zgSolve1.GraphPane.AddCurve("divide", p, Color.Red, SymbolType.Circle);
            this.listBox1.SelectedIndex = 0;
            this.zgSolve1.AxisChange();
            this.zgSolve1.Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            this.dgv.DataSource = null;
            this.dgv.DataSource = r[lb.Items[lb.SelectedIndex].ToString()];
        }

        private delegate void DrawCurvesDelegate(Dictionary<string, RKResults> results);
        internal void DrawCurves(Dictionary<string, RKResults> results)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DrawCurvesDelegate(this.DrawCurves), results);
            }
            else
            {
                foreach (string s in results.Keys)
                {
                    results[s].ForEach(r => this.curves[s].Add(r.Y[0], r.Y[1]));
                }
            }
        }

        //-------------------------

        private Dictionary<string, int> pCountList = new Dictionary<string, int>();

        private void mfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = sender as ListBox;
            dgvGama.RowCount = 1;
            dgvGama.ColumnCount = list.SelectedIndex + 1;
            dgvParameters.RowCount = list.SelectedIndex + 2;
        }

        private void sfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = sender as ListBox;
            dgvParameters.ColumnCount = this.pCountList[(list.Items[list.SelectedIndex] as string).ToUpper()];

        }

        string curveName = "line";
        string curveName1 = "line1";
        string curveName2 = "line2";
        private RAlgSolver ra1;
        private RAlgSolver ra2;


        private RKResults res = null;
        private TaskWorker tw = null;
        private TaskParameters tps = null;
        private Functions_2_2 funcDescr = null;
        private int n;
        private object tblResList;
        private int setCount;
        Vector startVector;

        private void btnSolveRK_Click(object sender, EventArgs e)
        {
            this.rtbResult1.Text = this.rtbResult2.Text = string.Empty;
            this.dgvTabRes.DataSource = null;
            this.n = int.Parse(this.txtN.Text);
            this.setCount = this.rbN1.Checked ? 1 : 2;
            //this.gamma = double.Parse(txtGamma.Text);

            ZedGraph.GraphPane gp = this.zgcMainChart2.GraphPane;
            gp.CurveList.Clear();
            this.curves.Clear();

            this.curves.Add(this.curveName, new PointPairList());
            gp.AddCurve(this.curveName, this.curves[curveName], Color.Black, SymbolType.None);


            List<double> gammaList = new List<double>();
            List<double> p = new List<double>();

            for (int i = 0; i < dgvGama.ColumnCount; i++)
            {
                gammaList.Add(Convert.ToDouble(dgvGama.Rows[0].Cells[i].Value));
            }

            List<TaskParameter> list = new List<TaskParameter>();
            for (int i = 0; i < dgvParameters.RowCount; i++)
            {
                TaskParameter tp = new TaskParameter(dgvParameters.ColumnCount);
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    double val = Convert.ToDouble(dgvParameters.Rows[i].Cells[j].Value);
                    tp.Param[j] = val;
                }
                list.Add(tp);
            }

            this.tps = new TaskParameters(list, gammaList);
            string mfName = this.setCount == 1 ? "MF1" : "MF2";
            this.funcDescr = new Functions_2_2(mfName, this.fnames[(sfList.SelectedItem as string).ToUpper()]);

            this.tw = new TaskWorker(tps, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);


            this.startVector = this.setCount == 1 ? new Vector(1, double.Parse(this.txtY0.Text)) :
                new Vector(2, double.Parse(this.txtY00.Text), double.Parse(this.txtY01.Text));

            RKVectorForm rk = new RKVectorForm(tw, curveName, double.Parse(this.txtT0.Text), double.Parse(this.txtT1.Text), startVector);
            this.res = rk.SolveWithConstH(n, RKMetodType.RK4_1);

            if (this.rbN1.Checked)
            {
                res.ForEach(r => this.curves[curveName].Add(r.X, r.Y[0]));
            }
            else
            {
                res.ForEach(r => this.curves[curveName].Add(r.Y[0], r.Y[1]));
            }
            this.zgcMainChart2.AxisChange();
            this.zgcMainChart2.Refresh();


            if (this.setCount == 1)
            {
                this.tblResList = new List<ResPointViewType1>();
                for (int i = 0; i < res.Count - 1; i++)
                    (this.tblResList as List<ResPointViewType1>).Add(new ResPointViewType1(res[i].X, res[i].Y[0], -1, -1));
                this.dgvTabRes.DataSource = null;
                this.dgvTabRes.DataSource = tblResList;
            }
            else
            {
                this.tblResList = new List<ResPointViewType2>();
                for (int i = 0; i < res.Count - 2; i++)
                    (this.tblResList as List<ResPointViewType2>).Add(new ResPointViewType2(res[i].X, res[i].Y[0], res[i].Y[1], -1, -1));
                this.dgvTabRes.DataSource = null;
                this.dgvTabRes.DataSource = tblResList;
            }
            this.dgvTabRes.RefreshDataSource();

            this.randomStartParameters = this.GetRandomStartParam();
        }

        private Dictionary<double, int> GetTT(RKResults res, int[] itab)
        {
            Dictionary<double, int> tt = new Dictionary<double, int>();
            if (this.setCount == 1)
            {
                for (int i = 0; i < res.Count - 1; i++)
                {
                    tt.Add(res[i].X, itab[i] - 1);
                }
            }
            else
            {
                for (int i = 0; i < res.Count - 2; i++)
                {
                    tt.Add(res[i].X, itab[i] - 1);
                }
            }
            return tt;
        }

        private List<List<double>> GetRandomStartParam()
        {
            List<List<double>> sp = new List<List<double>>();
            Random rand = new Random();
            for (int i = 0; i < mfList.SelectedIndex + 2; i++)
            {
                sp.Add(new List<double>());
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    //double val = rand.Next(10);
                    //val /= 5 * rand.NextDouble() > 0.5 ? 1 : -1;
                    double val = rand.NextDouble();//* rand.NextDouble() > 0.5 ? 1 : -1;
                    sp[i].Add(val);
                }
            }
            return sp;
        }

        private List<List<double>> randomStartParameters;

        private void btnSolveRAlg1_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnSolveRAlg1.Enabled = false;
                int tick = Environment.TickCount;

                double[] dz1 = new double[res.Count - 1];
                for (int i = 1; i < res.Count; i++)
                {
                    dz1[i - 1] = (res[i].Y[0] - res[i - 1].Y[0]) / (res[i].X - res[i - 1].X);
                }
                double[] dz2 = new double[res.Count - 2];
                if (this.setCount == 2)
                {
                    for (int i = 1; i < dz1.Length; i++)
                    {
                        dz2[i - 1] = (dz1[i] - dz1[i - 1]) / (res[i].X - res[i - 1].X);
                    }
                }

                //List<List<double>> sp = this.GetRandomStartParam();
                List<List<double>> sp = this.randomStartParameters;

                RAlgSolver ra = this.setCount == 1 ? new RAlgSolver(tw, res, sp, dz1) : new RAlgSolver(tw, res, sp, dz2);
                ra.FUNCT = new RAlgSolver.FUNCTDelegate(ra.FUNCT3);
                ra.R_Algorithm();

                for (int i = 0; i < res.Count - this.setCount; i++)
                {
                    if (this.setCount == 1)
                    {
                        (this.tblResList as List<ResPointViewType1>)[i].M1 = ra.itab[i];
                    }
                    else
                    {
                        (this.tblResList as List<ResPointViewType2>)[i].M1 = ra.itab[i];
                    }
                }
                this.dgvTabRes.RefreshDataSource();


                StringBuilder sb = new StringBuilder();

                Dictionary<double, int> tt = this.GetTT(this.res, ra.itab);

                //-- draw res ---
                if (!this.curves.ContainsKey(this.curveName1))
                {
                    this.curves.Add(this.curveName1, new PointPairList());
                    LineItem curve = new LineItem(this.curveName1, this.curves[curveName1], Color.Green, SymbolType.None);
                    curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                    curve.Line.Width = 2;
                    this.zgcMainChart2.GraphPane.CurveList.Add(curve);

                }
                else
                {
                    this.curves[this.curveName1].Clear();
                }

                int k = 0;
                List<TaskParameter> listDraw = new List<TaskParameter>();
                for (int i = 0; i < tps.Count; i++)
                {
                    TaskParameter tpDraw = new TaskParameter(tps[i].Param.Length);
                    string line = string.Format("{0}) ", i);
                    for (int j = 0; j < tps[0].Param.Length; j++)
                    {
                        tpDraw.Param[j] = ra.x[k];
                        line += string.Format("a{0}={1:f6} ", j, ra.x[k]);
                        k++;
                    }
                    sb.AppendLine(line);
                    listDraw.Add(tpDraw);
                }
                sb.AppendLine("-----");
                sb.Append(string.Format("f={0:f6}", ra.f));

                rtbResult1.Text = sb.ToString();

                TaskParameters tps1 = new TaskParameters(listDraw, tt);
                TaskWorker tw1 = new TaskWorker(tps1, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);
                RKVectorForm rk1 = new RKVectorForm(tw1, curveName1, double.Parse(this.txtT0.Text), double.Parse(this.txtT1.Text), this.startVector);
                RKResults res1 = rk1.SolveWithConstH(n, RKMetodType.RK2_1);

                //if (this.setCount == 1)
                //{
                //    res1.ForEach(r => this.curves[curveName1].Add(r.X, r.Y[0]));
                //}
                //else
                //{
                //    res1.ForEach(r => this.curves[curveName1].Add(r.Y[0], r.Y[1]));
                //}
                int nn = this.setCount == 1 ? 1 : 2;
                for (int i = 0; i < res1.Count - nn; i++)
                {
                    if (nn == 1)
                    {
                        this.curves[curveName1].Add(res1[i].X, res1[i].Y[0]);
                    }
                    else
                    {
                        this.curves[curveName1].Add(res1[i].Y[0], res1[i].Y[1]);
                    }
                }
                this.zgcMainChart2.AxisChange();
                this.zgcMainChart2.Refresh();
                //--------------
                this.ra1 = ra;

                double t = ((Environment.TickCount - tick) / (double)1000);

                this.rtbResult1.Text += string.Format("\r\n-----\r\ntime = {0} sec", t);
                this.inf1 = string.Format("Result: f = {0:f6} time = {1} sec", ra.f, t);
            }
            finally
            {
                this.btnSolveRAlg1.Enabled = true;
            }
        }

        private void btnSolveRAlg2_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnSolveRAlg2.Enabled = false;
                int tick = Environment.TickCount;
                this.inf2 = null;
                //List<List<double>> sp = this.GetRandomStartParam();
                List<List<double>> sp = this.randomStartParameters;

                RAlgSolver ra = new RAlgSolver(this.tw, this.res, sp, this.startVector);
                ra.FUNCT = new RAlgSolver.FUNCTDelegate(ra.FUNCT4);
                ra.R_Algorithm();

                for (int i = 0; i < res.Count - this.setCount; i++)
                {
                    if (this.setCount == 1)
                    {
                        (this.tblResList as List<ResPointViewType1>)[i].M2 = ra.itab[i];
                    }
                    else
                    {
                        (this.tblResList as List<ResPointViewType2>)[i].M2 = ra.itab[i];
                    }
                }
                this.dgvTabRes.RefreshDataSource();

                Dictionary<double, int> tt = this.GetTT(this.res, ra.itab);

                StringBuilder sb = new StringBuilder();

                //-- draw res ---
                if (!this.curves.ContainsKey(this.curveName2))
                {
                    this.curves.Add(this.curveName2, new PointPairList());
                    LineItem curve = new LineItem(this.curveName2, this.curves[curveName2], Color.Blue, SymbolType.None);
                    curve.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
                    curve.Line.Width = 2;
                    this.zgcMainChart2.GraphPane.CurveList.Add(curve);
                }
                else
                {
                    this.curves[this.curveName2].Clear();
                }

                int k = 0;
                List<TaskParameter> listDraw = new List<TaskParameter>();
                for (int i = 0; i < tps.Count; i++)
                {
                    TaskParameter tpDraw = new TaskParameter(tps[i].Param.Length);
                    string line = string.Format("{0}) ", i);
                    for (int j = 0; j < tps[0].Param.Length; j++)
                    {
                        tpDraw.Param[j] = ra.x[k];
                        line += string.Format("a{0}={1:f6} ", j, ra.x[k]);
                        k++;
                    }
                    sb.AppendLine(line);
                    listDraw.Add(tpDraw);
                }
                sb.AppendLine("-----");
                sb.Append(string.Format("f={0}", ra.f));
                rtbResult2.Text = sb.ToString();

                TaskParameters tps1 = new TaskParameters(listDraw, tt);
                TaskWorker tw1 = new TaskWorker(tps1, this.funcDescr.GetType(), this.funcDescr.MainFuncName, this.funcDescr.SecFuncName, this.funcDescr);
                RKVectorForm rk1 = new RKVectorForm(tw1, curveName2, double.Parse(this.txtT0.Text), double.Parse(this.txtT1.Text), this.startVector);
                RKResults res1 = rk1.SolveWithConstH(n, RKMetodType.RK2_1);

                //if (this.setCount == 1)
                //{
                //    res1.ForEach(r => this.curves[this.curveName2].Add(r.X, r.Y[0]));
                //}
                //else
                //{
                //    res1.ForEach(r => this.curves[this.curveName2].Add(r.Y[0], r.Y[1]));
                //}
                int nn = this.setCount == 1 ? 1 : 2;
                for (int i = 0; i < res1.Count - nn; i++)
                {
                    if (nn == 1)
                    {
                        this.curves[curveName2].Add(res1[i].X, res1[i].Y[0]);
                    }
                    else
                    {
                        this.curves[curveName2].Add(res1[i].Y[0], res1[i].Y[1]);
                    }
                }
                this.zgcMainChart2.AxisChange();
                this.zgcMainChart2.Refresh();

                this.ra2 = ra;
                double t = ((Environment.TickCount - tick) / (double)1000);

                this.rtbResult2.Text += string.Format("\r\n-----\r\ntime = {0} sec", t);
                this.inf2 = string.Format("Result: f = {0:f6} time = {1} sec", ra.f, t);
            }
            finally
            {
                this.btnSolveRAlg2.Enabled = true;
            }

        }
        private string inf1 = null;
        private string inf2 = null;

        private void btnResults_Click(object sender, EventArgs e)
        {
            double[,] startMas = new double[dgvParameters.RowCount, dgvParameters.ColumnCount];
            double[,] resMas1 = new double[dgvParameters.RowCount, dgvParameters.ColumnCount];
            double[,] resMas2 = new double[dgvParameters.RowCount, dgvParameters.ColumnCount];


            int k = 0;
            for (int i = 0; i < dgvParameters.RowCount; i++)
            {
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    startMas[i, j] = Convert.ToDouble(dgvParameters.Rows[i].Cells[j].Value);
                    resMas1[i, j] = ra1 != null ? ra1.x[k] : double.NaN;
                    resMas2[i, j] = ra2 != null ? ra2.x[k] : double.NaN;
                    k++;
                }
            }

            ResultForm rf = new ResultForm(this.curves.ContainsKey(this.curveName) ? this.curves[this.curveName] : null,
                this.curves.ContainsKey(this.curveName1) ? this.curves[this.curveName1] : null,
                this.curves.ContainsKey(this.curveName2) ? this.curves[this.curveName2] : null,
                startMas, resMas1, resMas2, dgvParameters.RowCount, dgvParameters.ColumnCount, this.inf1, this.inf2,
                this.randomStartParameters);
            rf.Show();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            Task td = this.LoadTaskInfo();
            this.view.taskCollection.Add(td);
            this.view.SaveTaskCollection(this.view.taskCollection);

            this.lstTasks.Items.Add(this.view.taskCollection.Count() - 1);
        }

        private Task LoadTaskInfo()
        {
            List<double> gamma = new List<double>();
            Vector startV;
            if (this.rbN1.Checked)
            {
                startV = new Vector(1, double.Parse(this.txtY0.Text));
            }
            else
            {
                startV = new Vector(2, double.Parse(this.txtY00.Text), double.Parse(this.txtY01.Text));
            }
            int k = 0;
            List<List<double>> sp = new List<List<double>>();
            for (int i = 0; i < dgvParameters.RowCount; i++)
            {
                sp.Add(new List<double>());
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    sp[i].Add(Convert.ToDouble(dgvParameters.Rows[i].Cells[j].Value));
                }
            }
            for (int i = 0; i < this.dgvGama.ColumnCount; i++)
            {
                gamma.Add(Convert.ToDouble(this.dgvGama.Rows[0].Cells[i].Value));
            }
            Task td = new Task()
            {
                t0 = double.Parse(this.txtT0.Text),
                t1 = double.Parse(this.txtT1.Text),
                gammaSet = gamma,
                startPoint = startV,
                parametrs = sp
            };
            return td;
        }

        private void lstTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = sender as ListBox;

            if (list.SelectedIndex >= 0)
            {
                Task task = this.view.taskCollection.ElementAt(list.SelectedIndex);

                this.txtT0.Text = task.t0.ToString();
                this.txtT1.Text = task.t1.ToString();
                this.dgvGama.RowCount = 1;
                this.dgvGama.ColumnCount = task.gammaSet.Count;
                for (int i = 0; i < task.gammaSet.Count; i++)
                {
                    this.dgvGama.Rows[0].Cells[i].Value = task.gammaSet[i];
                }
                if (task.startPoint.Count == 1)
                {
                    this.txtY0.Text = task.startPoint[0].ToString();
                    this.rbN1.Checked = true;
                }
                else
                {
                    this.rbN2.Checked = true;
                    this.txtY00.Text = task.startPoint[0].ToString();
                    this.txtY01.Text = task.startPoint[1].ToString();
                }

                this.dgvParameters.RowCount = task.parametrs.Count;
                this.dgvParameters.ColumnCount = task.parametrs[0].Count;

                for (int i = 0; i < task.parametrs.Count; i++)
                {
                    for (int j = 0; j < task.parametrs.Count; j++)
                    {
                        this.dgvParameters.Rows[i].Cells[j].Value = task.parametrs[i][j];
                    }
                }
            }
        }

        private void btnDelTask_Click(object sender, EventArgs e)
        {
            ListBox list = lstTasks;
            if (list.SelectedIndex >= 0)
            {
                this.view.taskCollection.RemoveAt(list.SelectedIndex);
                this.view.SaveTaskCollection(this.view.taskCollection);
                this.lstTasks.Items.RemoveAt(list.SelectedIndex);
            }
            else
            {
                if (list.Items.Count > 0)
                {
                    MessageBox.Show("You must choose item.");
                }
            }
        }

        private void btnEditTask_Click(object sender, EventArgs e)
        {
            if (lstTasks.SelectedIndex >= 0)
            {
                Task t = LoadTaskInfo();
                this.view.taskCollection[lstTasks.SelectedIndex] = t;
                this.view.SaveTaskCollection(this.view.taskCollection);
            }
        }
    }

}
