using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Common.Mathematic;
using Common.Task;
using Common.Wawe;
using Core;
using DevExpress.XtraGrid.Views.Base;
using SmartXLS;
using ZedGraph;

namespace View.Forms
{
    public partial class MainForm : Form
    {
        private readonly Dictionary<string, PointPairList> KursCurves = new Dictionary<string, PointPairList>();
        private readonly Dictionary<string, PointPairList> curves1 = new Dictionary<string, PointPairList>();
        private readonly Dictionary<string, PointPairList> curves2 = new Dictionary<string, PointPairList>();
        private readonly Dictionary<string, string> fnames = new Dictionary<string, string>();
        private readonly Dictionary<string, int> pCountList = new Dictionary<string, int>();
        private List<double> ASignal = new List<double>();
        private string aproxLine = "aprox";
        public string curveName = "line";
        public string curveName1 = "line1";
        public string curveName2 = "line2";
        public string curveName3 = "line3";
        public string curveNameSuff = "_1";
        public Dictionary<string, PointPairList> curves = new Dictionary<string, PointPairList>();
        public CultureInfo enUsCulture = new CultureInfo("en-US");
        private Functions_2_2 funcDescr;
        private string inf1;
        private string inf2;
        private string inf3;
        private bool isReal;
        private int n;

        private Dictionary<string, List<ResPointViewType2>> r;
        private RAlgSolver ra1;
        private RAlgSolver ra2;
        private RAlgSolver ra3;
        private List<List<double>> randomStartParameters;
        private RKResults res;
        private MainFuncSetCount selectedMainFuncSetCount;
        private SecondFuncType selectedSecondFuncType;
        public int setCount;
        private Vector startVector;
        private int step;
        private object tblResList;
        private TaskParameters tps;
        private TaskWorker tw;
        public View view;

        public MainForm(View view)
        {
            InitializeComponent();
            this.view = view;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            view.ViewActionCall(ViewEventType.Exit);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            view.ViewActionCall(ViewEventType.Exit);
        }

        private void btnGo1_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
            view.ConfigGraphPane(zgc1, this, PaneLayout.SingleRow, new List<ChartInfo>
                                                                       {
                                                                           new ChartInfo
                                                                               {
                                                                                   Title = "Depends t with z",
                                                                                   XAxisTitle = "t",
                                                                                   YAxisTitle = "z"
                                                                               }
                                                                       });
            GraphPane gp = zgc1.MasterPane[0];
            gp.CurveList.Clear();

            string fname = listFunctions1.SelectedItem.ToString();
            int randn = int.Parse(txtRandomCount.Text);
            curves.Clear();
            var r = new Random();
            for (int i = 0; i < randn; i++)
            {
                string curveName = string.Format("curve{0}", i);
                curves.Add(curveName, new PointPairList());
                Color c = Color.Black; //Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
                gp.AddCurve(curveName, curves[curveName], c, SymbolType.None);
                view.ViewActionCall(ViewEventType.StartSolving1, fname, curveName);
            }
        }

        private void btnGo2_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);

            var chInfo = new List<ChartInfo>();
            chInfo.Add(new ChartInfo {Title = "Depends (t,z1)", XAxisTitle = "t", YAxisTitle = "z1"});
            chInfo.Add(new ChartInfo {Title = "Depends (t,z2)", XAxisTitle = "t", YAxisTitle = "z2"});
            chInfo.Add(new ChartInfo {Title = "Depends (z1,z2)", XAxisTitle = "z1", YAxisTitle = "z2"});

            view.ConfigGraphPane(zgc1, this, PaneLayout.ExplicitCol21, chInfo);

            GraphPane gp = zgc1.MasterPane[0];
            GraphPane gp1 = zgc1.MasterPane[1];
            GraphPane gp2 = zgc1.MasterPane[2];
            gp.CurveList.Clear();
            gp1.CurveList.Clear();
            gp2.CurveList.Clear();

            string fname = listFunctions2.SelectedItem.ToString();
            int randonCount = int.Parse(txtRandomCount.Text);
            curves.Clear();
            curves1.Clear();
            curves2.Clear();
            //Random r = new Random();
            var ss = new string[randonCount,randonCount];
            for (int i = 0; i < randonCount; i++)
            {
                for (int j = 0; j < randonCount; j++)
                {
                    string curveName = string.Format("curve_{0}_{1}", i, j);
                    curves.Add(curveName, new PointPairList());
                    curves1.Add(curveName, new PointPairList());
                    curves2.Add(curveName, new PointPairList());

                    Color c = Color.Black; //Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));

                    gp.AddCurve(curveName, curves[curveName], c, SymbolType.None);
                    gp1.AddCurve(curveName, curves1[curveName], c, SymbolType.None);
                    gp2.AddCurve(curveName, curves2[curveName], c, SymbolType.None);
                    ss[i, j] = curveName;
                }
            }

            view.ViewActionCall(ViewEventType.StartSolving2, fname, ss);


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
            curves[curveName].Add(x, val[0]);
            RefreshaAllCharts(1);
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
                btnGo1.Enabled = false;
            }
            if (listFunctions2.Items.Count > 0)
            {
                listFunctions2.SelectedIndex = 0;
            }
            else
            {
                btnGo2.Enabled = false;
            }

            view.UpdateAlgorithmParameters(txtT0.Text, txtT1.Text, txtY0.Text, txtY00.Text, txtY01.Text, txtN.Text,
                                           txtRandomCount.Text, txtz1min.Text, txtz1max.Text, txtz2min.Text,
                                           txtz2max.Text);

            //this.view.ConfigGraphPane(this.zgc1.GraphPane, "Depends z1 with t", "t", "z1");
            //this.view.ConfigGraphPane(this.zgc2.GraphPane, "Depends z2 with t", "t", "z2");
            //this.view.ConfigGraphPane(this.zgc3.GraphPane, "Depends z1 with z2", "z1", "z2");

            view.ConfigGraphPane(zgSolve1, this, PaneLayout.SingleRow, new List<ChartInfo>
                                                                           {
                                                                               new ChartInfo
                                                                                   {
                                                                                       Title = "Depends t with z",
                                                                                       XAxisTitle = "t",
                                                                                       YAxisTitle = "z"
                                                                                   }
                                                                           });

            Type type = typeof (Functions_2_2);

            MethodInfo[] methods = type.GetMethods();
            fnames.Clear();
            foreach (MethodInfo m in methods)
            {
                object[] atr = m.GetCustomAttributes(typeof (FAtrAttribute), false);
                if (atr != null && atr.Length > 0)
                {
                    if ((atr[0] as FAtrAttribute).funcType == FuncType.Second)
                    {
                        sfList.Items.Add((atr[0] as FAtrAttribute).funcName);
                        pCountList.Add((atr[0] as FAtrAttribute).funcName.ToUpper(), (atr[0] as FAtrAttribute).PCount);
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
            foreach (TaskDescription t in view.taskCollection)
            {
                lstTasks.Items.Add(t.name);
                i++;
            }
            comboDeepness.SelectedIndex = 0;
            comboWaweOrderAprox.SelectedIndex = 0;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            view.UpdateAlgorithmParameters(txtT0.Text.Replace(',', '.'), txtT1.Text.Replace(',', '.'),
                                           txtY0.Text.Replace(',', '.'),
                                           txtY00.Text.Replace(',', '.'), txtY01.Text.Replace(',', '.'),
                                           txtN.Text, txtRandomCount.Text,
                                           txtz1min.Text.Replace(',', '.'), txtz1max.Text.Replace(',', '.'),
                                           txtz2min.Text.Replace(',', '.'), txtz2max.Text.Replace(',', '.'));
        }

        internal void DrawPoint2(double p, List<double> list, string curveName)
        {
            curves[curveName].Add(p, list[0]);
            curves1[curveName].Add(p, list[1]);
            curves2[curveName].Add(list[0], list[1]);
            RefreshaAllCharts(2);
        }


        private void btnSolvePodhod2_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
            listBox1.Items.Clear();


            GraphPane gp = zgSolve1.GraphPane;
            gp.CurveList.Clear();
            curves.Clear();

            string fname = listFunctions1.SelectedItem.ToString();

            string curveName = "Line";
            curves.Add(curveName, new PointPairList());
            Color c = Color.Black; //Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            gp.AddCurve(curveName, curves[curveName], c, SymbolType.None);
            view.ViewActionCall(ViewEventType.SolvePodhod2Type1, fname, curveName);
        }

        internal void ShowResultType1(List<double> divT, List<double> divZ, List<ResPointViewType1> list, double t0,
                                      double t)
        {
            dgv.DataSource = null;
            dgv.DataSource = list;

            if (divT.Count != divZ.Count)
                throw new ArgumentException("WTF?");

            for (int i = 0; i < divZ.Count; i++)
            {
                zgSolve1.GraphPane.AddCurve(i.ToString(), new PointPairList(), Color.Blue, SymbolType.None);
                zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(t0, divZ[i]);
                zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(t, divZ[i]);
            }

            zgSolve1.AxisChange();
            zgSolve1.Refresh();

            //MessageBox.Show(string.Format("t={0} : z={1}",divT,divZ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
            listBox1.Items.Clear();


            GraphPane gp = zgSolve1.GraphPane;
            gp.CurveList.Clear();
            curves.Clear();

            string fname = listFunctions2.SelectedItem.ToString();

            string curveName = "Line2";
            curves.Add(curveName, new PointPairList());
            Color c = Color.DarkBlue; //Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            gp.AddCurve(curveName, curves[curveName], c, SymbolType.None);
            view.ViewActionCall(ViewEventType.SolovePodhod2Type2, fname, curveName);
        }

        internal void DrawPhasePortret(double p, List<double> list, string curveName)
        {
            curves[curveName].Add(list[0], list[1]);
            zgSolve1.AxisChange();
            zgSolve1.Refresh();
        }

        internal void ShowResultType2(List<double> divT, List<double> divZ, List<ResPointViewType2> list, double z2min,
                                      double z2max)
        {
            dgv.DataSource = null;
            dgv.DataSource = list;

            if (divT.Count != divZ.Count)
                throw new ArgumentException("WTF?");

            for (int i = 0; i < divZ.Count; i++)
            {
                zgSolve1.GraphPane.AddCurve(i.ToString(), new PointPairList(), Color.Blue, SymbolType.None);
                zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(divZ[i], z2min);
                zgSolve1.GraphPane.CurveList[i.ToString()].AddPoint(divZ[i], z2max);
            }

            zgSolve1.AxisChange();
            zgSolve1.Refresh();
        }

        private void btnSolvingType2Mass_Click(object sender, EventArgs e)
        {
            btnUpdate_Click(null, null);
            listBox1.Items.Clear();
            GraphPane gp = zgSolve1.GraphPane;
            gp.CurveList.Clear();
            curves.Clear();
            string fname = listFunctions2.SelectedItem.ToString();

            int randonCount = int.Parse(txtRandomCount.Text);
            var curveNames = new string[randonCount,randonCount];
            for (int i = 0; i < randonCount; i++)
            {
                for (int j = 0; j < randonCount; j++)
                {
                    string curveName = string.Format("Line-{0}_{1}", i, j);
                    curves.Add(curveName, new PointPairList());
                    Color c = Color.DarkBlue; //Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
                    gp.AddCurve(curveName, curves[curveName], c, SymbolType.None);
                    curveNames[i, j] = curveName;
                }
            }
            view.ViewActionCall(ViewEventType.SolovePodhod2Type2Mass, fname, curveNames);
        }

        internal void ShowResultType2Mass(PointPairList p, Dictionary<string, List<ResPointViewType2>> r)
        {
            listBox1.Items.Clear();
            this.r = r;
            foreach (string s in r.Keys)
            {
                listBox1.Items.Add(s);
            }
            //p.Sort(SortType.XValues);
            zgSolve1.GraphPane.AddCurve("divide", p, Color.Red, SymbolType.Circle);
            listBox1.SelectedIndex = 0;
            zgSolve1.AxisChange();
            zgSolve1.Refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lb = sender as ListBox;
            dgv.DataSource = null;
            dgv.DataSource = r[lb.Items[lb.SelectedIndex].ToString()];
        }

        internal void DrawCurves(Dictionary<string, RKResults> results)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Dictionary<string, RKResults>>(DrawCurves), results);
            }
            else
            {
                foreach (string s in results.Keys)
                {
                    results[s].ForEach(r => curves[s].Add(r.Y[0], r.Y[1]));
                }
            }
        }

        //-------------------------

        private void mfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = sender as ListBox;
            dgvGama.RowCount = 1;
            dgvGama.ColumnCount = list.SelectedIndex + 1;
            dgvParameters.RowCount = list.SelectedIndex + 2;

            switch (list.SelectedIndex)
            {
                case 0:
                    selectedMainFuncSetCount = MainFuncSetCount.Two;
                    break;
                case 1:
                    selectedMainFuncSetCount = MainFuncSetCount.Three;
                    break;
                case 2:
                    selectedMainFuncSetCount = MainFuncSetCount.Four;
                    break;
                case 3:
                    selectedMainFuncSetCount = MainFuncSetCount.Five;
                    break;
            }
        }

        private void sfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = sender as ListBox;
            dgvParameters.ColumnCount = pCountList[(list.Items[list.SelectedIndex] as string).ToUpper()];
            switch (list.SelectedIndex)
            {
                case 0:
                    selectedSecondFuncType = SecondFuncType.Liner;
                    break;
                case 1:
                    selectedSecondFuncType = SecondFuncType.Square;
                    break;
                case 2:
                    selectedSecondFuncType = SecondFuncType.PeriodT;
                    break;
                case 3:
                    selectedSecondFuncType = SecondFuncType.PeriodZ;
                    break;
            }
        }

        private void btnSolveRK_Click(object sender, EventArgs e)
        {
            try
            {
                btnSolveRK.Enabled = false;
                isReal = false;
                view.Output.ClearScreen();
                grcTabRes.DataSource = null;
                n = int.Parse(txtN.Text, enUsCulture);
                setCount = rbN1.Checked ? 1 : 2;
                //this.gamma = double.Parse(txtGamma.Text);

                if (setCount == 1)
                {
                    view.ConfigGraphPane(zgcMainChart2, this, PaneLayout.SingleColumn, new List<ChartInfo>
                                                                                           {
                                                                                               new ChartInfo
                                                                                                   {
                                                                                                       Title = "(t,z1)",
                                                                                                       XAxisTitle = "t",
                                                                                                       YAxisTitle = "z1"
                                                                                                   },
                                                                                           });
                }
                else
                {
                    view.ConfigGraphPane(zgcMainChart2, this, PaneLayout.SingleColumn, new List<ChartInfo>
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
                //ZedGraph.GraphPane gp = this.zgcMainChart2.GraphPane;
                //gp.CurveList.Clear();
                curves.Clear();


                if (setCount == 1)
                {
                    curves.Add(curveName, new PointPairList());
                    zgcMainChart2.MasterPane[0].AddCurve(curveName, curves[curveName], Color.Black, SymbolType.None);
                }
                else
                {
                    curves.Add(curveName, new PointPairList());
                    curves.Add(curveName + curveNameSuff, new PointPairList());
                    zgcMainChart2.MasterPane[0].AddCurve(curveName + curveNameSuff, curves[curveName + curveNameSuff],
                                                         Color.Black, SymbolType.None);
                    zgcMainChart2.MasterPane[1].AddCurve(curveName, curves[curveName], Color.Black, SymbolType.None);
                }

                var p = new List<double>();

                var gammaList = new List<double>();
                for (int i = 0; i < dgvGama.ColumnCount; i++)
                {
                    double d = double.Parse(dgvGama.Rows[0].Cells[i].Value.ToString().Replace(',', '.'), enUsCulture);
                    gammaList.Add(d);
                }

                var list = new List<TaskParameter>();
                for (int i = 0; i < dgvParameters.RowCount; i++)
                {
                    var tp = new TaskParameter(dgvParameters.ColumnCount);
                    for (int j = 0; j < dgvParameters.ColumnCount; j++)
                    {
                        double val = double.Parse(dgvParameters.Rows[i].Cells[j].Value.ToString().Replace(',', '.'),
                                                  enUsCulture);
                        tp.Param[j] = val;
                    }
                    list.Add(tp);
                }

                tps = new TaskParameters(list, gammaList);
                string mfName = setCount == 1 ? "MF1" : "MF2";
                funcDescr = new Functions_2_2(mfName, fnames[(sfList.SelectedItem as string).ToUpper()]);

                tw = new TaskWorker(tps, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);


                startVector = setCount == 1
                                  ? new Vector(1, double.Parse(txtY0.Text.Replace(',', '.'), enUsCulture))
                                  : new Vector(2, double.Parse(txtY00.Text.Replace(',', '.'), enUsCulture),
                                               double.Parse(txtY01.Text.Replace(',', '.'), enUsCulture));

                var rk = new RKVectorForm(tw, curveName, double.Parse(txtT0.Text.Replace(',', '.'), enUsCulture),
                                          double.Parse(txtT1.Text.Replace(',', '.'), enUsCulture), startVector);
                res = rk.SolveWithConstH(n, RKMetodType.RK4_1);

                if (rbN1.Checked)
                {
                    for (int i = 0; i < res.Count - 1; i++)
                    {
                        curves[curveName].Add(res[i].X, res[i].Y[0]);
                    }
                    //res.ForEach(r => this.curves[curveName].Add(r.X, r.Y[0]));
                }
                else
                {
                    for (int i = 0; i < res.Count - 2; i++)
                    {
                        curves[curveName + curveNameSuff].Add(res[i].X, res[i].Y[0]);
                        curves[curveName].Add(res[i].Y[0], res[i].Y[1]);
                    }
                    //res.ForEach(r => this.curves[curveName].Add(r.Y[0], r.Y[1]));
                }
                zgcMainChart2.AxisChange();
                zgcMainChart2.Refresh();

                if (setCount == 1)
                {
                    tblResList = new List<ResPointViewType1>();
                    for (int i = 0; i < res.Count - 1; i++)
                        (tblResList as List<ResPointViewType1>).Add(new ResPointViewType1(res[i].X, res[i].Y[0], -1, -1));
                    grcTabRes.DataSource = null;
                    grcTabRes.DataSource = tblResList;
                }
                else
                {
                    tblResList = new List<ResPointViewType2>();
                    for (int i = 0; i < res.Count - 2; i++)
                        (tblResList as List<ResPointViewType2>).Add(new ResPointViewType2(res[i].X, res[i].Y[0],
                                                                                          res[i].Y[1], -1, -1));
                    grcTabRes.DataSource = null;
                    grcTabRes.DataSource = tblResList;
                }
                grcTabRes.RefreshDataSource();
            }
            finally
            {
                btnSolveRK.Enabled = true;
            }
        }

        private Dictionary<double, int> GetTT(RKResults res, int[] itab)
        {
            var tt = new Dictionary<double, int>();
            if (setCount == 1)
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
            if (InvokeRequired)
            {
                return (List<List<double>>) Invoke(new Func<List<List<double>>>(_GetRandomParam));
            }
            else
            {
                return _GetRandomParam();
            }
        }

        private List<List<double>> _GetRandomParam()
        {
            var sp = new List<List<double>>();
            var rand = new Random();
            for (int i = 0; i < mfList.SelectedIndex + 2; i++)
            {
                sp.Add(new List<double>());
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    //double val = rand.Next(10);
                    //val /= 5 * rand.NextDouble() > 0.5 ? 1 : -1;
                    double val = rand.NextDouble()*(rand.NextDouble() > 0.5 ? 1 : -1);
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
            return s/(r2.Count - n);
        }

        private RKResults DrawRes(RKResults r, RAlgSolver ra, string cn, string cnsuff, Color c, OutputHelper output,
                                  int eb, out double functional)
        {
            if (!isReal)
            {
                for (int i = 0; i < r.Count - setCount; i++)
                {
                    if (eb == 1)
                    {
                        if (setCount == 1)
                        {
                            (tblResList as List<ResPointViewType1>)[i + i*step].M1 = ra.itab[i];
                        }
                        else
                        {
                            (tblResList as List<ResPointViewType2>)[i + i*step].M1 = ra.itab[i];
                        }
                    }
                    else
                    {
                        if (setCount == 1)
                        {
                            (tblResList as List<ResPointViewType1>)[i + i*step].M2 = ra.itab[i];
                        }
                        else
                        {
                            (tblResList as List<ResPointViewType2>)[i + i*step].M2 = ra.itab[i];
                        }
                    }
                }
                grcTabRes.RefreshDataSource();
            }

            var sb = new StringBuilder();

            Dictionary<double, int> tt = GetTT(r, ra.itab);

            //-- draw res ---
            if (!curves.ContainsKey(cn))
            {
                curves.Add(cn, new PointPairList());
                var curve = new LineItem(cn, curves[cn], c, SymbolType.None);
                curve.Line.Style = DashStyle.Dash;
                curve.Line.Width = 2;
                //this.zgcMainChart2.MasterPane[1].CurveList.Add(curve);

                if (setCount == 2)
                {
                    curves.Add(cn + cnsuff, new PointPairList());
                    var curve1 = new LineItem(cn + cnsuff, curves[cn + cnsuff], c, SymbolType.None);
                    curve.Line.Style = DashStyle.Dash;
                    curve.Line.Width = 2;
                    zgcMainChart2.MasterPane[0].CurveList.Add(curve1);

                    zgcMainChart2.MasterPane[1].CurveList.Add(curve);
                }
                else
                {
                    zgcMainChart2.MasterPane[0].CurveList.Add(curve);
                }
            }
            else
            {
                curves[cn].Clear();
                if (setCount == 2)
                {
                    curves[cn + cnsuff].Clear();
                }
            }

            int k = 0;
            var listDraw = new List<TaskParameter>();
            for (int i = 0; i < tps.Count; i++)
            {
                var tpDraw = new TaskParameter(tps[i].Param.Length);
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

            var tps1 = new TaskParameters(listDraw, tt);
            var tw1 = new TaskWorker(tps1, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);

            double t0, t1;
            if (!isReal)
            {
                t0 = double.Parse(txtT0.Text);
                t1 = double.Parse(txtT1.Text);
            }
            else
            {
                t0 = 0;
                t1 = ASignal.Count*0.01;
            }
            var rk1 = new RKVectorForm(tw1, cn, t0, t1, startVector);
            RKResults res1 = rk1.SolveWithConstH(res.Count - 1, RKMetodType.RK2_1);

            functional = GetDiff(res, res1);
            sb.Append(string.Format("f={0:f6}", functional));
            //rtb.Text = sb.ToString();
            //this.SetControlFeathe(rtb, "text=", sb.ToString());
            output.WriteLine(sb.ToString());

            int nn = setCount == 1 ? 1 : 2;
            for (int i = 0; i < res1.Count - nn; i++)
            {
                if (nn == 1)
                {
                    curves[cn].Add(res1[i].X, res1[i].Y[0]);
                }
                else
                {
                    curves[cn].Add(res1[i].Y[0], res1[i].Y[1]);
                    curves[cn + cnsuff].Add(res1[i].X, res1[i].Y[0]);
                }
            }
            SetControlProperty(zgcMainChart2, "chart", null);
            //this.zgcMainChart2.AxisChange();
            //this.zgcMainChart2.Refresh();
            return res1;
        }

        public void SetControlProperty(Control c, string t, object value)
        {
            if (c.InvokeRequired)
            {
                c.BeginInvoke(new Action<Control, string, object>(SetControlProperty), c, t, value);
            }
            else
            {
                switch (t)
                {
                    case "enabled":
                        c.Enabled = (bool) value;
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

        private void btnSolveRAlg1_Click(object sender, EventArgs e)
        {
            var th = new Thread(x =>
                                    {
                                        try
                                        {
                                            if (randomStartParameters == null)
                                            {
                                                GenerateStartParam();
                                            }
                                            view.Output.WriteLine("SolveRalg1 started at " + DateTime.Now.ToString());
                                            SetControlProperty(btnSolveRAlg1, "enabled", false);
                                            int tick = Environment.TickCount;

                                            RKResults r = res.Trancate(step);

                                            var dz1 = new double[r.Count - 1];
                                            for (int i = 1; i < r.Count; i++)
                                            {
                                                dz1[i - 1] = (r[i].Y[0] - r[i - 1].Y[0])/(r[i].X - r[i - 1].X);
                                            }

                                            var dz2 = new double[r.Count - 2];
                                            if (setCount == 2)
                                            {
                                                for (int i = 1; i < dz1.Length; i++)
                                                {
                                                    dz2[i - 1] = (dz1[i] - dz1[i - 1])/(r[i].X - r[i - 1].X);
                                                }
                                            }

                                            List<List<double>> sp = randomStartParameters;

                                            RAlgSolver ra = setCount == 1
                                                                ? new RAlgSolver(tw, r, sp, dz1)
                                                                : new RAlgSolver(tw, r, sp, dz2);
                                            ra.FUNCT = ra.FUNCT3;
                                            ra.R_Algorithm();

                                            double functional;
                                            DrawRes(r, ra, curveName1, curveNameSuff, Color.Green, view.Output, 1,
                                                    out functional);

                                            ra1 = ra;

                                            double t = ((Environment.TickCount - tick)/(double) 1000);
                                            view.Output.WriteLine(string.Format(
                                                "Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                                            inf1 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
                                        }
                                        catch (ApplicationException ae)
                                        {
                                            MessageBox.Show(ae.Message);
                                        }
                                        finally
                                        {
                                            SetControlProperty(btnSolveRAlg1, "enabled", true);
                                            //this.btnSolveRAlg1.Enabled = true;
                                        }
                                    }
                )
                         {
                             Name = "RAlgSolve1",
                             IsBackground = true
                         };
            th.Start();
        }

        private void btnSolveRAlg2_Click(object sender, EventArgs e)
        {
            var th = new Thread(x =>
                                    {
                                        try
                                        {
                                            if (randomStartParameters == null)
                                            {
                                                GenerateStartParam();
                                            }
                                            view.Output.WriteLine("SolveRalg2 started at " + DateTime.Now.ToString());
                                            SetControlProperty(btnSolveRAlg2, "enabled", false);
                                            int tick = Environment.TickCount;
                                            inf2 = null;
                                            //List<List<double>> sp = this.GetRandomStartParam();
                                            List<List<double>> sp = randomStartParameters;

                                            RKResults r = res.Trancate(step);

                                            var ra = new RAlgSolver(tw, r, sp, startVector, 0);
                                            ra.FUNCT = ra.FUNCT4;
                                            ra.R_Algorithm();
                                            double functional;
                                            DrawRes(r, ra, curveName2, curveNameSuff, Color.Blue, view.Output, 2,
                                                    out functional);
                                            ra2 = ra;
                                            double t = ((Environment.TickCount - tick)/(double) 1000);
                                            view.Output.WriteLine(string.Format(
                                                "Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                                            inf2 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
                                        }
                                        catch (ApplicationException ae)
                                        {
                                            MessageBox.Show(ae.Message);
                                        }
                                        finally
                                        {
                                            SetControlProperty(btnSolveRAlg2, "enabled", true);
                                        }
                                    })
                         {
                             Name = "MainForm.RAlgSolve2",
                             IsBackground = true
                         };
            th.Start();
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            var startMas = new double[dgvParameters.RowCount,dgvParameters.ColumnCount];
            var resMas1 = new double[dgvParameters.RowCount,dgvParameters.ColumnCount];
            var resMas2 = new double[dgvParameters.RowCount,dgvParameters.ColumnCount];


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

            var rf = new ResultForm(this, startMas, resMas1, resMas2, dgvParameters.RowCount, dgvParameters.ColumnCount,
                                    inf1, inf2, randomStartParameters);
            rf.Show();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            using (var f = new EnterNameForm())
            {
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    TaskDescription td = LoadTaskInfo(false);
                    td.name = f.name;
                    view.taskCollection.Add(td);
                    view.SaveTaskCollection(view.taskCollection);

                    lstTasks.Items.Add(td.name);
                }
            }
        }

        private TaskDescription LoadTaskInfo(bool isEdit)
        {
            var gamma = new List<double>();
            Vector startV;
            if (rbN1.Checked)
            {
                startV = new Vector(1, double.Parse(txtY0.Text.Replace(',', '.'), enUsCulture));
            }
            else
            {
                startV = new Vector(2, double.Parse(txtY00.Text.Replace(',', '.'), enUsCulture),
                                    double.Parse(txtY01.Text.Replace(',', '.'), enUsCulture));
            }
            //int k = 0;
            var sp = new List<List<double>>();
            for (int i = 0; i < dgvParameters.RowCount; i++)
            {
                sp.Add(new List<double>());
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    sp[i].Add(Convert.ToDouble(dgvParameters.Rows[i].Cells[j].Value));
                }
            }
            for (int i = 0; i < dgvGama.ColumnCount; i++)
            {
                gamma.Add(double.Parse(dgvGama.Rows[0].Cells[i].Value.ToString().Replace(',', '.'), enUsCulture));
            }
            var td = new TaskDescription
                         {
                             t0 = double.Parse(txtT0.Text.Replace(',', '.'), enUsCulture),
                             t1 = double.Parse(txtT1.Text.Replace(',', '.'), enUsCulture),
                             gammaSet = gamma,
                             startPoint = startV,
                             parametrs = sp,
                             mainFuncSetCount = selectedMainFuncSetCount,
                             secFuncType = selectedSecondFuncType
                         };
            if (isEdit)
            {
                td.name = lstTasks.SelectedItem.ToString();
            }
            return td;
        }

        private void lstTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            var list = sender as ListBox;

            if (list.SelectedIndex >= 0)
            {
                TaskDescription task = view.taskCollection.ElementAt(list.SelectedIndex);

                txtT0.Text = task.t0.ToString();
                txtT1.Text = task.t1.ToString();
                dgvGama.RowCount = 1;
                dgvGama.ColumnCount = task.gammaSet.Count;
                for (int i = 0; i < task.gammaSet.Count; i++)
                {
                    dgvGama.Rows[0].Cells[i].Value = task.gammaSet[i];
                }
                if (task.startPoint.Count == 1)
                {
                    txtY0.Text = task.startPoint[0].ToString();
                    rbN1.Checked = true;
                }
                else
                {
                    rbN2.Checked = true;
                    txtY00.Text = task.startPoint[0].ToString();
                    txtY01.Text = task.startPoint[1].ToString();
                }

                dgvParameters.RowCount = task.parametrs.Count;
                dgvParameters.ColumnCount = task.parametrs[0].Count;

                for (int i = 0; i < task.parametrs.Count; i++)
                {
                    for (int j = 0; j < task.parametrs[i].Count; j++)
                    {
                        dgvParameters.Rows[i].Cells[j].Value = task.parametrs[i][j];
                    }
                }

                sfList.SelectedIndex = (int) task.secFuncType;

                mfList.SelectedIndex = (int) task.mainFuncSetCount;
            }
        }

        private void btnDelTask_Click(object sender, EventArgs e)
        {
            ListBox list = lstTasks;
            if (list.SelectedIndex >= 0)
            {
                view.taskCollection.RemoveAt(list.SelectedIndex);
                view.SaveTaskCollection(view.taskCollection);
                lstTasks.Items.RemoveAt(list.SelectedIndex);
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
                TaskDescription t = LoadTaskInfo(true);
                view.taskCollection[lstTasks.SelectedIndex] = t;
                view.SaveTaskCollection(view.taskCollection);
            }
        }

        private void openFD_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                ASignal.Clear();
                KursCurves.Clear();
                comboSignalsAprox.Items.Clear();

                var wb = new WorkBook();
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
            var comboBox = sender as ComboBox;
            string key = comboBox.SelectedItem.ToString();
            zedAprox.GraphPane.CurveList.Clear();
            var line = new LineItem(key, KursCurves[key], Color.Blue, SymbolType.None);
            zedAprox.GraphPane.CurveList.Add(line);
            if (KursCurves.ContainsKey(aproxLine))
            {
                KursCurves.Remove(aproxLine);
            }
            zedAprox.AxisChange();
            zedAprox.Refresh();

            var Wawe = new CWaweTransform();
            var Signal = new List<double>();
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

        private void btnAprox_Click(object sender, EventArgs e)
        {
            try
            {
                var Wawe = new CWaweTransform();
                var Signal = new List<double>();
                foreach (PointPair p in KursCurves[comboSignalsAprox.SelectedItem.ToString()])
                {
                    Signal.Add(p.Y);
                }
                ASignal = Wawe.GetAproxKoef(Signal, comboWaweOrderAprox.SelectedIndex + 1,
                                            comboDeepness.SelectedIndex + 1);

                if (!KursCurves.ContainsKey(aproxLine))
                {
                    KursCurves.Add(aproxLine, new PointPairList());
                    zedAprox.GraphPane.CurveList.Add(new LineItem(aproxLine, KursCurves[aproxLine], Color.Red,
                                                                  SymbolType.Circle));
                }
                KursCurves[aproxLine].Clear();
                double dt = Signal.Count/ASignal.Count;
                for (int t = 0; t < ASignal.Count; t++)
                {
                    //curveAAA.AddPoint((t * dt+(t+1)*dt)/2, Math.Pow(A.Count, 0.5) * A[t]);

                    KursCurves[aproxLine].Add(t*dt, Math.Pow(ASignal.Count, 0.5)*ASignal[t]);
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
            isReal = true;
            view.Output.ClearScreen();
            grcTabRes.DataSource = null;
            n = int.Parse(txtN.Text);
            setCount = rbN1.Checked ? 1 : 2;

            double t = 0;
            var r = new RKResults();
            foreach (double y in ASignal)
            {
                r.Add(new RKResult(t, new Vector(1, y)));
                t += 0.01;
            }
            res = r;
            randomStartParameters = GetRandomStartParam();

            var gammaList = new List<double>();
            for (int i = 0; i < dgvGama.ColumnCount; i++)
            {
                gammaList.Add(Convert.ToDouble(dgvGama.Rows[0].Cells[i].Value));
            }

            var list = new List<TaskParameter>();
            for (int i = 0; i < dgvParameters.RowCount; i++)
            {
                var tp = new TaskParameter(dgvParameters.ColumnCount);
                for (int j = 0; j < dgvParameters.ColumnCount; j++)
                {
                    double val = Convert.ToDouble(dgvParameters.Rows[i].Cells[j].Value);
                    tp.Param[j] = val;
                }
                list.Add(tp);
            }


            tps = new TaskParameters(list, gammaList);
            string mfName = setCount == 1 ? "MF1" : "MF2";
            funcDescr = new Functions_2_2(mfName, fnames[(sfList.SelectedItem as string).ToUpper()]);
            startVector = new Vector(1, 0);
            tw = new TaskWorker(tps, funcDescr.GetType(), funcDescr.MainFuncName, funcDescr.SecFuncName, funcDescr);

            xtcMain.SelectedTabPageIndex = 2;
        }

        private void btnGenerateStartParam_Click(object sender, EventArgs e)
        {
            GenerateStartParam();
        }

        private void GenerateStartParam()
        {
            randomStartParameters = GetRandomStartParam();
            var sb = new StringBuilder();
            view.Output.WriteLine("RandomStartParam generated :");
            foreach (var list in randomStartParameters)
            {
                string s = string.Empty;
                foreach (double d in list)
                {
                    s += string.Format("{0:f6}  ", d);
                }
                sb.AppendLine(s);
            }
            view.Output.WriteLine(sb.ToString());
        }

        internal void DrawResult(RKResults res, string curve)
        {
            if (res[0].Y.Count == 1)
            {
                res.ForEach(r => curves[curve].Add(r.X, r.Y[0]));
            }
            else
            {
                res.ForEach(r => curves[curve].Add(r.X, r.Y[0]));
                res.ForEach(r => curves1[curve].Add(r.X, r.Y[1]));
                res.ForEach(r => curves2[curve].Add(r.Y[0], r.Y[1]));
            }
            zgc1.AxisChange();
            zgc1.Refresh();
        }

        private void btnSolveRAlg3_Click(object sender, EventArgs e)
        {
            double alpha;
            if (double.TryParse(txtAlpha.Text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentUICulture, out alpha))
            {
                var th = new Thread(x =>
                                        {
                                            try
                                            {
                                                view.Output.WriteLine("SolveRalg3 started at " + DateTime.Now.ToString());

                                                if (randomStartParameters == null)
                                                {
                                                    GenerateStartParam();
                                                }

                                                double a = alpha;

                                                SetControlProperty(btnSolveRAlg3, "enabled", false);

                                                int tick = Environment.TickCount;
                                                inf3 = null;
                                                List<List<double>> sp = randomStartParameters;

                                                var ra = new RAlgSolver(tw, res, sp, startVector, 1);
                                                ra.FUNCT = ra.FUNCT4;
                                                ra.alpha = a;
                                                ra.R_Algorithm();

                                                double functional;
                                                DrawRes(res, ra, curveName3, curveNameSuff, Color.DeepPink, view.Output,
                                                        2, out functional);

                                                ra3 = ra;
                                                double t = ((Environment.TickCount - tick)/(double) 1000);
                                                view.Output.WriteLine(
                                                    string.Format("Result: f = {0:f6} time = {1} sec\r\n", functional, t));
                                                inf3 = string.Format("Result: f = {0:f6} time = {1} sec", functional, t);
                                            }
                                            catch (ApplicationException ae)
                                            {
                                                MessageBox.Show(ae.Message);
                                            }
                                            finally
                                            {
                                                SetControlProperty(btnSolveRAlg3, "enabled", true);
                                            }
                                        })
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

        private void btnSolveAll_Click(object sender, EventArgs e)
        {
        }

        private void speStep_EditValueChanged(object sender, EventArgs e)
        {
            step = Convert.ToInt32(speStep.EditValue);
        }

        private void grvTabRes_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "M1" || e.Column.FieldName == "M2")
            {
                int v = Convert.ToInt32(e.CellValue);
                if (v <= 0)
                {
                    e.DisplayText = "";
                }
            }
        }
    }
}