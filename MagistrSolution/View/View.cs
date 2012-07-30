using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Mathematic;
using Common.Task;
using Core;
using View.Forms;
using ZedGraph;

namespace View
{
    public sealed class View : IView
    {
        public CultureInfo en = new CultureInfo("en-US");
        private MainForm mainForm;
        public TaskCollection taskCollection;
        private string tasksFileName = "tasks.txt";

        public View()
        {
            taskCollection = loadTaskCollection();
        }

        public OutputHelper Output { get; private set; }

        #region IView Members

        public TaskCollection loadTaskCollection()
        {
            var f = new FileInfo(tasksFileName);
            if (f.Exists)
            {
                using (StreamReader r = File.OpenText(tasksFileName))
                {
                    try
                    {
                        string s = r.ReadToEnd();
                        return taskCollection = JSONHelper.Deserialize<TaskCollection>(s);
                    }
                    finally
                    {
                        r.Close();
                    }
                }
            }
            else
            {
                return
                    new TaskCollection();
            }
        }

        public void SaveTaskCollection(TaskCollection tc)
        {
            var f = new FileInfo(tasksFileName);
            if (f.Exists)
            {
                f.Delete();
            }
            using (StreamWriter w = File.CreateText(tasksFileName))
            {
                try
                {
                    w.Write(JSONHelper.Serialize(tc));
                }
                finally
                {
                    w.Close();
                }
            }
        }

        public event Func<ViewEventType, ViewEventArgs, object> OnViewAction;


        public void ShowMainForm()
        {
            if (IsNullOrDisposed(mainForm))
            {
                mainForm = new MainForm(this);
            }
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action(ShowMainForm));
            }
            else
            {
                Output = new OutputHelper(mainForm.meOutput);
                mainForm.Show();
            }
        }

        public void DrawPoint(double p, List<double> list, string curveName)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<double, List<double>, string>(DrawPoint), p, list, curveName);
            }
            else
            {
                mainForm.DrawPoint(p, list, curveName);
            }
        }

        public void DrawPoint2(double p, List<double> list, string curveName)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<double, List<double>, string>(DrawPoint2), p, list, curveName);
            }
            else
            {
                mainForm.DrawPoint2(p, list, curveName);
            }
        }

        public void SendSolvingResultType1(RKResults res, IFunctionExecuter fe)
        {
            var dz = new double[res.Count - 1];
            for (int i = 1; i < res.Count; i++)
            {
                dz[i - 1] = (res[i].Y[0] - res[i - 1].Y[0])/(res[i].X - res[i - 1].X);
            }
            var lambda = new int[res.Count - 1];

            //v[0] = t <= 0 ? -1 : 1
            var valInPoint = new List<double>();
            for (int i = 0; i < res.Count - 1; i++)
            {
                valInPoint.Clear();
                for (int j = 0; j < fe.SetCount; j++)
                {
                    valInPoint.Add(Math.Abs(dz[i] - fe.FunctionSet(res[i].X, res[i].Y, j)));
                }

                lambda[i] = valInPoint.IndexOf(valInPoint.Min()) + 1;
            }
            //int g = 0;

            int mn = lambda[0];
            var divZ = new List<double>(); // res[0].Y[0];
            var divT = new List<double>(); // res[0].X;

            var checkedSet = new List<int>();

            for (int i = 1; i < lambda.Length; i++)
            {
                if (lambda[i] != mn && (checkedSet.Count == 0 || !checkedSet.Contains(lambda[i])))
                {
                    divT.Add((res[i].X + res[i - 1].X)/2f);
                    divZ.Add((res[i].Y[0] + res[i - 1].Y[0])/2f);
                    checkedSet.Add(mn);
                    mn = lambda[i];
                    continue;
                }
            }

            //fill DataSouce
            var list = new List<ResPointViewType1>();
            for (int i = 0; i < lambda.Length; i++)
            {
                list.Add(new ResPointViewType1(res[i].X, res[i].Y[0], lambda[i], -1));
            }

            ShowResultType1(divT, divZ, list, res[0].X, res[res.Count - 1].X);
        }


        public void DrawPhasePortret(double p, List<double> list, string curveName)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<double, List<double>, string>(DrawPoint2), p, list, curveName);
            }
            else
            {
                mainForm.DrawPhasePortret(p, list, curveName);
            }
        }

        public void SendSolvingResultType2(RKResults res, IFunctionExecuter fe)
        {
            List<double> divZ;
            List<double> divZ2;
            List<double> divT;
            List<ResPointViewType2> list;
            ResolveType2(res, fe, out divT, out divZ, out divZ2, out list);

            ShowResultType2(divT, divZ, list, res.MinimumZ2(), res.MaximumZ2());
        }

        public void SendSolvingResultType2Mass(Dictionary<string, RKResults> results, IFunctionExecuter fe)
        {
            List<double> divZ;
            List<double> divZ2;
            List<double> divT;
            List<ResPointViewType2> list;
            var p = new PointPairList();

            mainForm.DrawCurves(results);

            var r = new Dictionary<string, List<ResPointViewType2>>();


            foreach (string s in results.Keys)
            {
                ResolveType2(results[s], fe, out divT, out divZ, out divZ2, out list);

                r.Add(s, list);
                if (divZ.Count > 0)
                {
                    p.Add(divZ[0], divZ2[0]);
                }
            }

            p.Sort(SortType.YValues);
            //p.Sort(ZedGraph.SortType.XValues);


            ShowResultType2Mass(p, r);
        }

        public void ConfigGraphPane(ZedGraphControl zgc, Form f, PaneLayout pl, List<ChartInfo> chInfo)
        {
            MasterPane master = zgc.MasterPane;

            master.PaneList.Clear();
            master.Title.IsVisible = true;
            foreach (ChartInfo ci in chInfo)
            {
                var pane = new GraphPane();
                master.PaneList.Add(pane);
                ConfigGraphPane(pane, ci.Title, ci.XAxisTitle, ci.YAxisTitle);
            }
            using (Graphics g = zgc.CreateGraphics())
            {
                master.SetLayout(g, pl);
                master.AxisChange(g);
            }
            zgc.AxisChange();
        }

        public void UpdateAlgorithmParameters(string t0, string t1, string y0, string y00, string y01, string rkN,
                                              string randN, string z1min, string z1max, string z2min, string z2max)
        {
            try
            {
                ViewActionCall(ViewEventType.UpdateParams,
                               double.Parse(t0, en), double.Parse(t1, en),
                               double.Parse(y0, en),
                               double.Parse(y00, en), double.Parse(y01, en),
                               int.Parse(rkN, en),
                               int.Parse(randN, en),
                               double.Parse(z1min, en), double.Parse(z1max, en),
                               double.Parse(z2min, en), double.Parse(z2max, en));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        public void DrawResult(RKResults res, string curve)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new Action<RKResults, string>(DrawResult), res, curve);
            }
            else
            {
                mainForm.DrawResult(res, curve);
            }
        }

        #endregion

        private bool IsNullOrDisposed(Form f)
        {
            return f == null || f.IsDisposed;
        }

        public void ViewActionCall(ViewEventType type, params object[] p)
        {
            if (OnViewAction != null)
            {
                OnViewAction(type, new ViewEventArgs(p));
            }
        }

        internal void ShowResultType1(List<double> divT, List<double> divZ, object list, double t0, double t)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new ShowResultDelegate(ShowResultType1), divT, divZ, list, t0, t);
            }
            else
            {
                mainForm.ShowResultType1(divT, divZ, list as List<ResPointViewType1>, t0, t);
            }
        }

        internal void ShowResultType2(List<double> divT, List<double> divZ, object list, double z2min, double z2max)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(new ShowResultDelegate(ShowResultType2), divT, divZ, list, z2min, z2max);
            }
            else
            {
                mainForm.ShowResultType2(divT, divZ, list as List<ResPointViewType2>, z2min, z2max);
            }
        }

        private void ResolveType2(RKResults res, IFunctionExecuter fe, out List<double> divT, out List<double> divZ,
                                  out List<double> divZ2, out List<ResPointViewType2> list)
        {
            var dz1 = new double[res.Count - 1];
            for (int i = 1; i < res.Count; i++)
            {
                dz1[i - 1] = (res[i].Y[0] - res[i - 1].Y[0])/(res[i].X - res[i - 1].X);
            }
            var dz2 = new double[res.Count - 2];
            for (int i = 1; i < dz1.Length; i++)
            {
                dz2[i - 1] = (dz1[i] - dz1[i - 1])/(res[i].X - res[i - 1].X);
            }

            var lambda = new int[res.Count - 2];

            var valInPoint = new List<double>();
            for (int i = 0; i < res.Count - 2; i++)
            {
                valInPoint.Clear();
                for (int j = 0; j < fe.SetCount; j++)
                {
                    valInPoint.Add(Math.Abs(dz2[i] - fe.FunctionSet(res[i].X, res[i].Y, j)));
                }

                lambda[i] = valInPoint.IndexOf(valInPoint.Min()) + 1;
            }
            //int g = 0;

            int mn = lambda[0];
            divZ = new List<double>(); // res[0].Y[0];
            divT = new List<double>(); // res[0].X;
            divZ2 = new List<double>();

            var checkedSet = new List<int>();

            for (int i = 1; i < lambda.Length; i++)
            {
                if (lambda[i] != mn && (checkedSet.Count == 0 || !checkedSet.Contains(lambda[i])))
                {
                    divT.Add((res[i].X + res[i - 1].X)/2f);
                    divZ.Add((res[i].Y[0] + res[i - 1].Y[0])/2f);
                    divZ2.Add((res[i].Y[1] + res[i - 1].Y[1])/2f);
                    checkedSet.Add(mn);
                    mn = lambda[i];
                }
            }

            //fill DataSouce
            list = new List<ResPointViewType2>();
            for (int i = 0; i < lambda.Length; i++)
            {
                list.Add(new ResPointViewType2(res[i].X, res[i].Y[0], res[i].Y[1], lambda[i], -1));
            }
        }

        private void ShowResultType2Mass(PointPairList p, Dictionary<string, List<ResPointViewType2>> r)
        {
            if (mainForm.InvokeRequired)
            {
                mainForm.BeginInvoke(
                    new Action<PointPairList, Dictionary<string, List<ResPointViewType2>>>(ShowResultType2Mass), p, r);
            }
            else
            {
                mainForm.ShowResultType2Mass(p, r);
            }
        }

        private void ConfigGraphPane(GraphPane p, string title, string xAxisTitle, string yAxixTitle)
        {
            p.Legend.FontSpec.Size = 6;

            p.Legend.Position = LegendPos.Right;

            p.Title.Text = title;
            p.Title.FontSpec.Size = 14;

            p.XAxis.Title.Text = xAxisTitle;
            p.XAxis.Title.FontSpec.Size = 12;
            p.XAxis.Scale.FontSpec.Size = 10;

            p.YAxis.Title.Text = yAxixTitle;
            p.YAxis.Title.FontSpec.Size = 12;
            p.YAxis.Scale.FontSpec.Size = 10;
        }

        #region Nested type: ShowResultDelegate

        private delegate void ShowResultDelegate(List<double> divT, List<double> divZ, object list, double t0, double t);

        #endregion
    }
}