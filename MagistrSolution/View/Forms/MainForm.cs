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
using SmartXLS;
using Common.Wawe;

namespace View.Forms
{
    public partial class MainForm : Form
    {
        public View view;

        public Dictionary<string, PointPairList> curves = new Dictionary<string, PointPairList>();
        private Dictionary<string, PointPairList> curves1 = new Dictionary<string, PointPairList>();
        private Dictionary<string, PointPairList> curves2 = new Dictionary<string, PointPairList>();

        private Dictionary<string, List<ResPointViewType2>> r = null;

        public MainForm(View view)
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
            chInfo.Add(new Core.ChartInfo() { Title = "Depends (t,z2)", XAxisTitle = "t", YAxisTitle = "z2" });
            chInfo.Add(new Core.ChartInfo() { Title = "Depends (z1,z2)", XAxisTitle = "z1", YAxisTitle = "z2" });

            this.view.ConfigGraphPane(this.zgc1, this, PaneLayout.ExplicitCol21, chInfo);

            ZedGraph.GraphPane gp = this.zgc1.MasterPane[0];
            ZedGraph.GraphPane gp1 = this.zgc1.MasterPane[1];
            ZedGraph.GraphPane gp2 = this.zgc1.MasterPane[2];
            gp.CurveList.Clear();
            gp1.CurveList.Clear();
            gp2.CurveList.Clear();

            string fname = listFunctions2.SelectedItem.ToString();
            int randonCount = int.Parse(txtRandomCount.Text);
            this.curves.Clear();
            this.curves1.Clear();
            this.curves2.Clear();
            //Random r = new Random();
            string[,] ss = new string[randonCount, randonCount];
            for (int i = 0; i < randonCount; i++)
            {
                for (int j = 0; j < randonCount; j++)
                {
                    string curveName = string.Format("curve_{0}_{1}", i, j);
                    this.curves.Add(curveName, new PointPairList());
                    this.curves1.Add(curveName, new PointPairList());
                    this.curves2.Add(curveName, new PointPairList());

                    Color c = Color.Black;//Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));

                    gp.AddCurve(curveName, this.curves[curveName], c, SymbolType.None);
                    gp1.AddCurve(curveName, this.curves1[curveName], c, SymbolType.None);
                    gp2.AddCurve(curveName, this.curves2[curveName], c, SymbolType.None);
                    ss[i, j] = curveName;
                }
            }

            this.view.ViewActionCall(ViewEventType.StartSolving2, fname, ss);




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
                this.lstTasks.Items.Add(t.name);
                i++;
            }
            this.comboDeepness.SelectedIndex = 0;
            this.comboWaweOrderAprox.SelectedIndex = 0;
        }
        private Dictionary<string, string> fnames = new Dictionary<string, string>();

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.view.UpdateAlgorithmParameters(this.txtT0.Text.Replace(',', '.'), this.txtT1.Text.Replace(',', '.'),
                this.txtY0.Text.Replace(',', '.'),
                this.txtY00.Text.Replace(',', '.'), txtY01.Text.Replace(',', '.'),
                this.txtN.Text, this.txtRandomCount.Text,
                this.txtz1min.Text.Replace(',', '.'), this.txtz1max.Text.Replace(',', '.'),
                this.txtz2min.Text.Replace(',', '.'), this.txtz2max.Text.Replace(',', '.'));
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

        internal void DrawCurves(Dictionary<string, RKResults> results)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Dictionary<string, RKResults>>(this.DrawCurves), results);
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

            switch (list.SelectedIndex)
            {
                case 0:
                    this.selectedMainFuncSetCount = MainFuncSetCount.Two;
                    break;
                case 1:
                    this.selectedMainFuncSetCount = MainFuncSetCount.Three;
                    break;
                case 2:
                    this.selectedMainFuncSetCount = MainFuncSetCount.Four;
                    break;
                case 3:
                    this.selectedMainFuncSetCount = MainFuncSetCount.Five;
                    break;

            }

        }

        private void sfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox list = sender as ListBox;
            dgvParameters.ColumnCount = this.pCountList[(list.Items[list.SelectedIndex] as string).ToUpper()];
            switch (list.SelectedIndex)
            {
                case 0:
                    this.selectedSecondFuncType = SecondFuncType.Liner;
                    break;
                case 1:
                    this.selectedSecondFuncType = SecondFuncType.Square;
                    break;
                case 2:
                    this.selectedSecondFuncType = SecondFuncType.PeriodT;
                    break;
                case 3:
                    this.selectedSecondFuncType = SecondFuncType.PeriodZ;
                    break;
            }
        }

        public string curveName = "line";
        public string curveName1 = "line1";
        public string curveName2 = "line2";
        public string curveName3 = "line3";
        public string curveNameSuff = "_1";
        private RAlgSolver ra1;
        private RAlgSolver ra2;
        private RAlgSolver ra3;


        private RKResults res = null;
        private TaskWorker tw = null;
        private TaskParameters tps = null;
        private Functions_2_2 funcDescr = null;
        private int n;
        private object tblResList;
        public int setCount;
        Vector startVector;

        public System.Globalization.CultureInfo enUsCulture = new System.Globalization.CultureInfo("en-US");

        private void btnSolveRK_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnSolveRK.Enabled = false;
                this.isReal = false;
                this.view.Output.ClearScreen();
                this.dgvTabRes.DataSource = null;
                this.n = int.Parse(this.txtN.Text, enUsCulture);
                this.setCount = this.rbN1.Checked ? 1 : 2;
                //this.gamma = double.Parse(txtGamma.Text);

                if (this.setCount == 1)
                {
                    this.view.ConfigGraphPane(this.zgcMainChart2, this, PaneLayout.SingleColumn, new List<Core.ChartInfo>() 
            {
                new Core.ChartInfo(){ Title="(t,z1)", XAxisTitle="t", YAxisTitle="z1"},
            });
                }
                else
                {

                    this.view.ConfigGraphPane(this.zgcMainChart2, this, PaneLayout.SingleColumn, new List<Core.ChartInfo>() 
            {
                new Core.ChartInfo(){ Title="(t,z1)", XAxisTitle="t", YAxisTitle="z1"},
                new Core.ChartInfo(){ Title="(z1,z2)", XAxisTitle="z1", YAxisTitle="z2"}
            });
                }
                //ZedGraph.GraphPane gp = this.zgcMainChart2.GraphPane;
                //gp.CurveList.Clear();
                this.curves.Clear();


                if (this.setCount == 1)
                {
                    this.curves.Add(this.curveName, new PointPairList());
                    this.zgcMainChart2.MasterPane[0].AddCurve(this.curveName, this.curves[curveName], Color.Black, SymbolType.None);
                }
                else
                {
                    this.curves.Add(this.curveName, new PointPairList());
                    this.curves.Add(this.curveName + this.curveNameSuff, new PointPairList());
                    this.zgcMainChart2.MasterPane[0].AddCurve(this.curveName + this.curveNameSuff, this.curves[this.curveName + this.curveNameSuff], Color.Black, SymbolType.None);
                    this.zgcMainChart2.MasterPane[1].AddCurve(this.curveName, this.curves[curveName], Color.Black, SymbolType.None);
                }

                List<double> p = new List<double>();

                List<double> gammaList = new List<double>();
                for (int i = 0; i < dgvGama.ColumnCount; i++)
                {
                    double d = double.Parse(dgvGama.Rows[0].Cells[i].Value.ToString().Replace(',', '.'), enUsCulture);
                    gammaList.Add(d);
                }

                List<TaskParameter> list = new List<TaskParameter>();
                for (int i = 0; i < dgvParameters.RowCount; i++)
                {
                    TaskParameter tp = new TaskParameter(dgvParameters.ColumnCount);
                    for (int j = 0; j < dgvParameters.ColumnCount; j++)
                    {
                        double val = double.Parse(dgvParameters.Rows[i].Cells[j].Value.ToString().Replace(',', '.'), enUsCulture);
                        tp.Param[j] = val;
                    }
                    list.Add(tp);
                }

                this.tps = new TaskParameters(list, gammaList);
                string mfName = this.setCount == 1 ? "MF1" : "MF2";
                this.funcDescr = new Functions_2_2(mfName, this.fnames[(sfList.SelectedItem as string).ToUpper()]);

                this.tw = new TaskWorker(tps, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);


                this.startVector = this.setCount == 1 ? new Vector(1, double.Parse(this.txtY0.Text.Replace(',', '.'), enUsCulture)) :
                    new Vector(2, double.Parse(this.txtY00.Text.Replace(',', '.'), enUsCulture), double.Parse(this.txtY01.Text.Replace(',', '.'), enUsCulture));

                RKVectorForm rk = new RKVectorForm(tw, curveName, double.Parse(this.txtT0.Text.Replace(',', '.'), enUsCulture), double.Parse(this.txtT1.Text.Replace(',', '.'), enUsCulture), startVector);
                this.res = rk.SolveWithConstH(n, RKMetodType.RK4_1);

                if (this.rbN1.Checked)
                {
                    for (int i = 0; i < res.Count - 1; i++)
                    {
                        this.curves[curveName].Add(res[i].X, res[i].Y[0]);
                    }
                    //res.ForEach(r => this.curves[curveName].Add(r.X, r.Y[0]));
                }
                else
                {
                    for (int i = 0; i < res.Count - 2; i++)
                    {
                        this.curves[curveName + curveNameSuff].Add(res[i].X, res[i].Y[0]);
                        this.curves[curveName].Add(res[i].Y[0], res[i].Y[1]);
                    }
                    //res.ForEach(r => this.curves[curveName].Add(r.Y[0], r.Y[1]));
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
            }
            finally
            {
                this.btnSolveRK.Enabled = true;
            }
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
                    double val = rand.NextDouble() * (rand.NextDouble() > 0.5 ? 1 : -1);
                    sp[i].Add(val);
                }
            }
            return sp;
        }

        private double GetDiff(RKResults r1, RKResults r2)
        {
            double s = 0;
            int n = r1[0].Y.Count;
            for (int i = 0; i < r1.Count - n; i++)
            {
                double ss = 0;
                for (int j = 0; j < n; j++)
                {
                    ss += Math.Pow(r1[i].Y[j] - r2[i].Y[j], 2);
                }
                s += Math.Pow(ss, 0.5);
            }
            return s / (r2.Count - n);
        }

        private List<List<double>> randomStartParameters;
        bool isReal = false;

        private RKResults DrawRes(RAlgSolver ra, string cn, string cnsuff, Color c, OutputHelper output, int eb, out double functional)
        {
            if (!isReal)
            {
                for (int i = 0; i < res.Count - this.setCount; i++)
                {
                    if (eb == 1)
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
                    else
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
                }
                this.dgvTabRes.RefreshDataSource();
            }

            StringBuilder sb = new StringBuilder();

            Dictionary<double, int> tt = this.GetTT(this.res, ra.itab);

            //-- draw res ---
            if (!this.curves.ContainsKey(cn))
            {
                this.curves.Add(cn, new PointPairList());
                LineItem curve = new LineItem(cn, this.curves[cn], c, SymbolType.None);
                curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                curve.Line.Width = 2;
                //this.zgcMainChart2.MasterPane[1].CurveList.Add(curve);

                if (this.setCount == 2)
                {

                    this.curves.Add(cn + cnsuff, new PointPairList());
                    LineItem curve1 = new LineItem(cn + cnsuff, this.curves[cn + cnsuff], c, SymbolType.None);
                    curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                    curve.Line.Width = 2;
                    this.zgcMainChart2.MasterPane[0].CurveList.Add(curve1);

                    this.zgcMainChart2.MasterPane[1].CurveList.Add(curve);
                }
                else
                {
                    this.zgcMainChart2.MasterPane[0].CurveList.Add(curve);
                }

            }
            else
            {
                this.curves[cn].Clear();
                if (this.setCount == 2)
                {
                    this.curves[cn + cnsuff].Clear();
                }
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

            TaskParameters tps1 = new TaskParameters(listDraw, tt);
            TaskWorker tw1 = new TaskWorker(tps1, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);

            double t0, t1;
            if (!isReal)
            {
                t0 = double.Parse(this.txtT0.Text);
                t1 = double.Parse(this.txtT1.Text);
            }
            else
            {
                t0 = 0;
                t1 = ASignal.Count * 0.01;
            }
            RKVectorForm rk1 = new RKVectorForm(tw1, cn, t0, t1, this.startVector);
            RKResults res1 = rk1.SolveWithConstH(this.res.Count - 1, RKMetodType.RK2_1);

            functional = this.GetDiff(this.res, res1);
            sb.Append(string.Format("f={0:f6}", functional));
            //rtb.Text = sb.ToString();
            //this.SetControlFeathe(rtb, "text=", sb.ToString());
            output.WriteLine(sb.ToString());

            int nn = this.setCount == 1 ? 1 : 2;
            for (int i = 0; i < res1.Count - nn; i++)
            {
                if (nn == 1)
                {
                    this.curves[cn].Add(res1[i].X, res1[i].Y[0]);
                }
                else
                {
                    this.curves[cn].Add(res1[i].Y[0], res1[i].Y[1]);
                    this.curves[cn + cnsuff].Add(res1[i].X, res1[i].Y[0]);
                }
            }
            this.SetControlFeathe(this.zgcMainChart2, "chart", null);
            //this.zgcMainChart2.AxisChange();
            //this.zgcMainChart2.Refresh();
            return res1;
        }

        public void SetControlFeathe(Control c, string t, object value)
        {
            if (c.InvokeRequired)
            {
                c.BeginInvoke(new Action<Control, string, object>(this.SetControlFeathe), c, t, value);
            }
            else
            {
                switch (t)
                {
                    case "enabled":
                        c.Enabled = (bool)value;
                        break;
                    case "text=":
                        c.Text = value.ToString();
                        break;
                    case "text+=":
                        c.Text += value.ToString();
                        break;
                    case "chart":
                        (c as ZedGraphControl).AxisChange();
                        (c as ZedGraphControl).Refresh();
                        break;

                }
            }
        }

        public void SolveRalg1()
        {
            try
            {
                if (this.randomStartParameters == null)
                {
                    throw new ApplicationException("Generate start parameters");
                }
                this.view.Output.WriteLine("SolveRalg1 started at " + DateTime.Now.ToString());
                this.SetControlFeathe(this.btnSolveRAlg1, "enabled", false);
                //this.btnSolveRAlg1.Enabled = false;
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
                ra.FUNCT = new RAlgSolver.RAlgFunctionDelegate(ra.FUNCT3);
                ra.R_Algorithm();

                double functional;
                this.DrawRes(ra, this.curveName1, this.curveNameSuff, Color.Green, this.view.Output, 1, out functional);

                this.ra1 = ra;

                double t = ((Environment.TickCount - tick) / (double)1000);
                this.view.Output.WriteLine(string.Format("Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                this.inf1 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
            }
            catch (ApplicationException ae)
            {
                MessageBox.Show(ae.Message);
            }
            finally
            {
                this.SetControlFeathe(this.btnSolveRAlg1, "enabled", true);
                //this.btnSolveRAlg1.Enabled = true;
            }
        }

        private void btnSolveRAlg1_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(this.SolveRalg1))
            {
                Name = "RAlgSolve1",
                IsBackground = true
            };
            th.Start();
        }

        public void SolveRalg2()
        {
            try
            {
                if (this.randomStartParameters == null)
                {
                    throw new ApplicationException("Generate start parameters");
                }
                this.view.Output.WriteLine("SolveRalg2 started at " + DateTime.Now.ToString());
                this.SetControlFeathe(this.btnSolveRAlg2, "enabled", false);
                int tick = Environment.TickCount;
                this.inf2 = null;
                //List<List<double>> sp = this.GetRandomStartParam();
                List<List<double>> sp = this.randomStartParameters;

                RAlgSolver ra = new RAlgSolver(this.tw, this.res, sp, this.startVector, 0);
                ra.FUNCT = new RAlgSolver.RAlgFunctionDelegate(ra.FUNCT4);
                ra.R_Algorithm();
                double functional;
                this.DrawRes(ra, this.curveName2, this.curveNameSuff, Color.Blue, this.view.Output, 2, out functional);
                this.ra2 = ra;
                double t = ((Environment.TickCount - tick) / (double)1000);
                this.view.Output.WriteLine(string.Format("Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                this.inf2 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
            }
            catch (ApplicationException ae)
            {
                MessageBox.Show(ae.Message);
            }
            finally
            {
                this.SetControlFeathe(this.btnSolveRAlg2, "enabled", true);
            }
        }

        private void btnSolveRAlg2_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(this.SolveRalg2))
            {
                Name = "MainForm.RAlgSolve2",
                IsBackground = true
            };
            th.Start();

        }


        private string inf1 = null;
        private string inf2 = null;
        private string inf3 = null;

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

            ResultForm rf = new ResultForm(this, startMas, resMas1, resMas2, dgvParameters.RowCount, dgvParameters.ColumnCount,
                this.inf1, this.inf2, this.randomStartParameters);
            rf.Show();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            using (EnterNameForm f = new EnterNameForm())
            {
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {

                    Task td = this.LoadTaskInfo(false);
                    td.name = f.name;
                    this.view.taskCollection.Add(td);
                    this.view.SaveTaskCollection(this.view.taskCollection);

                    this.lstTasks.Items.Add(td.name);
                }
            }
        }

        private Task LoadTaskInfo(bool isEdit)
        {
            List<double> gamma = new List<double>();
            Vector startV;
            if (this.rbN1.Checked)
            {
                startV = new Vector(1, double.Parse(this.txtY0.Text.Replace(',', '.'), enUsCulture));
            }
            else
            {
                startV = new Vector(2, double.Parse(this.txtY00.Text.Replace(',', '.'), enUsCulture),
                    double.Parse(this.txtY01.Text.Replace(',', '.'), enUsCulture));
            }
            //int k = 0;
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
                gamma.Add(double.Parse(this.dgvGama.Rows[0].Cells[i].Value.ToString().Replace(',', '.'), enUsCulture));
            }
            Task td = new Task()
            {
                t0 = double.Parse(this.txtT0.Text.Replace(',', '.'), enUsCulture),
                t1 = double.Parse(this.txtT1.Text.Replace(',', '.'), enUsCulture),
                gammaSet = gamma,
                startPoint = startV,
                parametrs = sp,
                mainFuncSetCount = this.selectedMainFuncSetCount,
                secFuncType = this.selectedSecondFuncType
            };
            if (isEdit)
            {
                td.name = this.lstTasks.SelectedItem.ToString();
            }
            return td;
        }

        private SecondFuncType selectedSecondFuncType;
        private MainFuncSetCount selectedMainFuncSetCount;

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
                    for (int j = 0; j < task.parametrs[i].Count; j++)
                    {
                        this.dgvParameters.Rows[i].Cells[j].Value = task.parametrs[i][j];
                    }
                }

                this.sfList.SelectedIndex = (int)task.secFuncType;

                this.mfList.SelectedIndex = (int)task.mainFuncSetCount;
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
                Task t = LoadTaskInfo(true);
                this.view.taskCollection[lstTasks.SelectedIndex] = t;
                this.view.SaveTaskCollection(this.view.taskCollection);
            }
        }

        private List<double> ASignal = new List<double>();
        private Dictionary<string, PointPairList> KursCurves = new Dictionary<string, PointPairList>();

        private void openFD_FileOk(object sender, CancelEventArgs e)
        {
            try
            {

                ASignal.Clear();
                KursCurves.Clear();
                comboSignalsAprox.Items.Clear();

                WorkBook wb = new WorkBook();
                wb.readXLSX(openFD.FileName);
                DataTable dt = wb.ExportDataTable();
                double t = 0;
                double d = 0.01;
                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    string key = dt.Rows[0][j].ToString();
                    comboSignalsAprox.Items.Add(key);
                    KursCurves.Add(key, new PointPairList());
                    t = 0;
                    for (int i = 1; i < dt.Rows.Count; i++, t += d)
                    {
                        object o = dt.Rows[i][j];
                        KursCurves[key].Add(new PointPair(i, Convert.ToDouble(o)));

                    }
                }
                comboSignalsAprox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFD.ShowDialog();
        }

        private void comboSignalsAprox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string key = comboBox.SelectedItem.ToString();
            this.zedAprox.GraphPane.CurveList.Clear();
            LineItem line = new LineItem(key, this.KursCurves[key], Color.Blue, SymbolType.None);
            this.zedAprox.GraphPane.CurveList.Add(line);
            if (KursCurves.ContainsKey(this.aproxLine))
            {
                this.KursCurves.Remove(this.aproxLine);
            }
            this.zedAprox.AxisChange();
            this.zedAprox.Refresh();

            CWaweTransform Wawe = new CWaweTransform();
            List<double> Signal = new List<double>();
            foreach (PointPair p in KursCurves[comboSignalsAprox.SelectedItem.ToString()])
            {
                Signal.Add(p.Y);
            }
            CDecomposition D = Wawe.Decompose(Signal, 11, 2);
            D.Approx[0] = 0;
            D.DeNoise(Convert.ToDouble(textNoise.Text));
            Signal = Wawe.Reconstruct(D);

            comboDeepness.Items.Clear();
            int J = -1;
            for (int i = 1; i <= 15; i++)
            {
                J = i;
                if (Math.Pow(2, i) == Signal.Count)
                    break;
                comboDeepness.Items.Add(i);
            }
            comboDeepness.SelectedIndex = 0;
        }
        private string aproxLine = "aprox";

        private void btnAprox_Click(object sender, EventArgs e)
        {

            try
            {
                CWaweTransform Wawe = new CWaweTransform();
                List<double> Signal = new List<double>();
                foreach (PointPair p in KursCurves[comboSignalsAprox.SelectedItem.ToString()])
                {
                    Signal.Add(p.Y);
                }
                ASignal = Wawe.GetAproxKoef(Signal, comboWaweOrderAprox.SelectedIndex + 1, comboDeepness.SelectedIndex + 1);

                if (!KursCurves.ContainsKey(aproxLine))
                {
                    KursCurves.Add(aproxLine, new PointPairList());
                    zedAprox.GraphPane.CurveList.Add(new LineItem(aproxLine, KursCurves[aproxLine], Color.Red, SymbolType.Circle));
                }
                KursCurves[aproxLine].Clear();
                double dt = Signal.Count / ASignal.Count;
                for (int t = 0; t < ASignal.Count; t++)
                {
                    //curveAAA.AddPoint((t * dt+(t+1)*dt)/2, Math.Pow(A.Count, 0.5) * A[t]);

                    KursCurves[aproxLine].Add(t * dt, Math.Pow(ASignal.Count, 0.5) * ASignal[t]);
                }
                //zedAprox.GraphPane.XAxis.Scale.Max = Signal.Count;
                zedAprox.AxisChange();
                zedAprox.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnTransfer_Click(object sender, EventArgs e)
        {
            this.isReal = true;
            this.view.Output.ClearScreen();
            this.dgvTabRes.DataSource = null;
            this.n = int.Parse(this.txtN.Text);
            this.setCount = this.rbN1.Checked ? 1 : 2;

            double t = 0;
            RKResults r = new RKResults();
            foreach (double y in ASignal)
            {
                r.Add(new RKResult(t, new Vector(1, y)));
                t += 0.01;
            }
            this.res = r;
            this.randomStartParameters = this.GetRandomStartParam();

            List<double> gammaList = new List<double>();
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
            this.startVector = new Vector(1, 0);
            this.tw = new TaskWorker(tps, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);

            xtcMain.SelectedTabPageIndex = 2;
        }

        private void btnGenerateStartParam_Click(object sender, EventArgs e)
        {
            this.randomStartParameters = this.GetRandomStartParam();
            StringBuilder sb = new StringBuilder();
            this.view.Output.WriteLine("RandomStartParam generated :");
            foreach (List<double> list in this.randomStartParameters)
            {
                string s = string.Empty;
                foreach (double d in list)
                {
                    s += string.Format("{0:f6}  ", d);
                }
                sb.AppendLine(s);
            }
            this.view.Output.WriteLine(sb.ToString());
        }

        internal void DrawResult(RKResults res, string curve)
        {
            if (res[0].Y.Count == 1)
            {
                res.ForEach(r => this.curves[curve].Add(r.X, r.Y[0]));
            }
            else
            {
                res.ForEach(r => this.curves[curve].Add(r.X, r.Y[0]));
                res.ForEach(r => this.curves1[curve].Add(r.X, r.Y[1]));
                res.ForEach(r => this.curves2[curve].Add(r.Y[0], r.Y[1]));
            }
            this.zgc1.AxisChange();
            this.zgc1.Refresh();
        }

        private void btnSolveRAlg3_Click(object sender, EventArgs e)
        {

            double alpha;
            if (double.TryParse(this.txtAlpha.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentUICulture, out alpha))
            {
                Thread th = new Thread(new ParameterizedThreadStart(this.SolveRalg3))
            {
                Name = "MainForm.RAlgSolve3",
                IsBackground = true
            };
                th.Start(alpha);
            }
            else
            {
                MessageBox.Show("Error while parsing alpha");
            }

        }

        public void SolveRalg3(object alpha)
        {
            try
            {
                this.view.Output.WriteLine("SolveRalg3 started at " + DateTime.Now.ToString());

                if (this.randomStartParameters == null)
                {
                    throw new ApplicationException("Generate start parameters");
                }

                double a = (double)alpha;

                this.SetControlFeathe(this.btnSolveRAlg3, "enabled", false);

                int tick = Environment.TickCount;
                this.inf3 = null;
                List<List<double>> sp = this.randomStartParameters;

                RAlgSolver ra = new RAlgSolver(this.tw, this.res, sp, this.startVector, 1);
                ra.FUNCT = new RAlgSolver.RAlgFunctionDelegate(ra.FUNCT4);
                ra.alpha = a;
                ra.R_Algorithm();

                double functional;
                this.DrawRes(ra, this.curveName3, this.curveNameSuff, Color.DeepPink, this.view.Output, 2, out functional);

                this.ra3 = ra;
                double t = ((Environment.TickCount - tick) / (double)1000);
                this.view.Output.WriteLine(string.Format("Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                this.inf3 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
            }
            catch (ApplicationException ae)
            {
                MessageBox.Show(ae.Message);
            }
            finally
            {
                this.SetControlFeathe(this.btnSolveRAlg3, "enabled", true);
            }
        }


    }

}
