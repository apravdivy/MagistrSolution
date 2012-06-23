using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using Core;
using Common;
using View.Forms;
using Common.Mathematic;
using ZedGraph;
using System.IO;
using Common.Task;

namespace View
{
    public sealed class View : IView
    {
        public TaskCollection taskCollection;
        private string tasksFileName = "tasks.txt";
        private MainForm mainForm;
        public OutputHelper Output { get; private set; }

        public View()
        {
            this.taskCollection = this.loadTaskCollection();

        }

        public TaskCollection loadTaskCollection()
        {
            FileInfo f = new FileInfo(this.tasksFileName);
            if (f.Exists)
            {
                using (StreamReader r = File.OpenText(this.tasksFileName))
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
            FileInfo f = new FileInfo(this.tasksFileName);
            if (f.Exists)
            {
                f.Delete();
            }
            using (StreamWriter w = File.CreateText(this.tasksFileName))
            {
                try
                {
                    w.Write(JSONHelper.Serialize<TaskCollection>(tc));
                }
                finally
                {
                    w.Close();
                }
            }
        }

        private bool IsNullOrDisposed(Form f)
        {
            return f == null || f.IsDisposed;
        }

        public void ViewActionCall(ViewEventType type, params object[] p)
        {
            if (this.OnViewAction != null)
            {
                this.OnViewAction(type, new ViewEventArgs(p));
            }
        }

        #region IView Members

        public event Func<ViewEventType, ViewEventArgs, object> OnViewAction;


        public void ShowMainForm()
        {
            if (IsNullOrDisposed(this.mainForm))
            {
                this.mainForm = new MainForm(this);
            }
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action(this.ShowMainForm));
            }
            else
            {
                this.Output = new OutputHelper(mainForm.meOutput);
                this.mainForm.Show();
            }
        }

        public void DrawPoint(double p, List<double> list, string curveName)
        {
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action<double, List<double>, string>(this.DrawPoint), p, list, curveName);
            }
            else
            {
                this.mainForm.DrawPoint(p, list, curveName);
            }
        }

        public void DrawPoint2(double p, List<double> list, string curveName)
        {
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action<double, List<double>, string>(this.DrawPoint2), p, list, curveName);
            }
            else
            {
                this.mainForm.DrawPoint2(p, list, curveName);
            }
        }

        public void SendSolvingResultType1(RKResults res, IFunctionExecuter fe)
        {
            double[] dz = new double[res.Count - 1];
            for (int i = 1; i < res.Count; i++)
            {
                dz[i - 1] = (res[i].Y[0] - res[i - 1].Y[0]) / (res[i].X - res[i - 1].X);
            }
            int[] lambda = new int[res.Count - 1];

            //v[0] = t <= 0 ? -1 : 1
            List<double> valInPoint = new List<double>();
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
            List<double> divZ = new List<double>();// res[0].Y[0];
            List<double> divT = new List<double>();// res[0].X;

            List<int> checkedSet = new List<int>();

            for (int i = 1; i < lambda.Length; i++)
            {
                if (lambda[i] != mn && (checkedSet.Count == 0 || !checkedSet.Contains(lambda[i])))
                {
                    divT.Add((res[i].X + res[i - 1].X) / 2f);
                    divZ.Add((res[i].Y[0] + res[i - 1].Y[0]) / 2f);
                    checkedSet.Add(mn);
                    mn = lambda[i];
                    continue;
                }
            }

            //fill DataSouce
            List<ResPointViewType1> list = new List<ResPointViewType1>();
            for (int i = 0; i < lambda.Length; i++)
            {
                list.Add(new ResPointViewType1(res[i].X, res[i].Y[0], lambda[i], -1));
            }

            this.ShowResultType1(divT, divZ, list, res[0].X, res[res.Count - 1].X);

        }

        private delegate void ShowResultDelegate(List<double> divT, List<double> divZ, object list, double t0, double t);
        internal void ShowResultType1(List<double> divT, List<double> divZ, object list, double t0, double t)
        {
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new ShowResultDelegate(this.ShowResultType1), divT, divZ, list, t0, t);
            }
            else
            {

                this.mainForm.ShowResultType1(divT, divZ, list as List<ResPointViewType1>, t0, t);
            }
        }

        internal void ShowResultType2(List<double> divT, List<double> divZ, object list, double z2min, double z2max)
        {
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new ShowResultDelegate(this.ShowResultType2), divT, divZ, list, z2min, z2max);
            }
            else
            {

                this.mainForm.ShowResultType2(divT, divZ, list as List<ResPointViewType2>, z2min, z2max);
            }
        }


        public void DrawPhasePortret(double p, List<double> list, string curveName)
        {
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action<double, List<double>, string>(this.DrawPoint2), p, list, curveName);
            }
            else
            {
                this.mainForm.DrawPhasePortret(p, list, curveName);
            }
        }

        public void SendSolvingResultType2(Common.Mathematic.RKResults res, IFunctionExecuter fe)
        {
            List<double> divZ;
            List<double> divZ2;
            List<double> divT;
            List<ResPointViewType2> list;
            this.ResolveType2(res, fe, out divT, out divZ, out divZ2, out list);

            this.ShowResultType2(divT, divZ, list, res.MinimumZ2(), res.MaximumZ2());
        }

        private void ResolveType2(Common.Mathematic.RKResults res, IFunctionExecuter fe, out List<double> divT, out List<double> divZ, out List<double> divZ2, out List<ResPointViewType2> list)
        {
            double[] dz1 = new double[res.Count - 1];
            for (int i = 1; i < res.Count; i++)
            {
                dz1[i - 1] = (res[i].Y[0] - res[i - 1].Y[0]) / (res[i].X - res[i - 1].X);
            }
            double[] dz2 = new double[res.Count - 2];
            for (int i = 1; i < dz1.Length; i++)
            {
                dz2[i - 1] = (dz1[i] - dz1[i - 1]) / (res[i].X - res[i - 1].X);
            }

            int[] lambda = new int[res.Count - 2];

            List<double> valInPoint = new List<double>();
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
            divZ = new List<double>();// res[0].Y[0];
            divT = new List<double>();// res[0].X;
            divZ2 = new List<double>();

            List<int> checkedSet = new List<int>();

            for (int i = 1; i < lambda.Length; i++)
            {
                if (lambda[i] != mn && (checkedSet.Count == 0 || !checkedSet.Contains(lambda[i])))
                {
                    divT.Add((res[i].X + res[i - 1].X) / 2f);
                    divZ.Add((res[i].Y[0] + res[i - 1].Y[0]) / 2f);
                    divZ2.Add((res[i].Y[1] + res[i - 1].Y[1]) / 2f);
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

        public void SendSolvingResultType2Mass(Dictionary<string, RKResults> results, IFunctionExecuter fe)
        {
            List<double> divZ;
            List<double> divZ2;
            List<double> divT;
            List<ResPointViewType2> list;
            ZedGraph.PointPairList p = new ZedGraph.PointPairList();

            this.mainForm.DrawCurves(results);

            Dictionary<string, List<ResPointViewType2>> r = new Dictionary<string, List<ResPointViewType2>>();



            foreach (string s in results.Keys)
            {
                this.ResolveType2(results[s], fe, out divT, out divZ, out divZ2, out list);

                r.Add(s, list);
                if (divZ.Count > 0)
                {
                    p.Add(divZ[0], divZ2[0]);
                }
            }

            p.Sort(ZedGraph.SortType.YValues);
            //p.Sort(ZedGraph.SortType.XValues);



            this.ShowResultType2Mass(p, r);

        }

        private void ShowResultType2Mass(ZedGraph.PointPairList p, Dictionary<string, List<ResPointViewType2>> r)
        {
            if (mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action<PointPairList, Dictionary<string, List<ResPointViewType2>>>(this.ShowResultType2Mass), p, r);
            }
            else
            {
                this.mainForm.ShowResultType2Mass(p, r);
            }
        }

        public void ConfigGraphPane(ZedGraphControl zgc, Form f, PaneLayout pl, List<ChartInfo> chInfo)
        {
            MasterPane master = zgc.MasterPane;

            master.PaneList.Clear();
            master.Title.IsVisible = true;
            foreach (ChartInfo ci in chInfo)
            {
                GraphPane pane = new GraphPane();
                master.PaneList.Add(pane);
                this.ConfigGraphPane(pane, ci.Title, ci.XAxisTitle, ci.YAxisTitle);
            }
            using (System.Drawing.Graphics g = zgc.CreateGraphics())
            {
                master.SetLayout(g, pl);
                master.AxisChange(g);
            }
            zgc.AxisChange();
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

        public System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");

        public void UpdateAlgorithmParameters(string t0, string t1, string y0, string y00, string y01, string rkN, string randN, string z1min, string z1max, string z2min, string z2max)
        {
            try
            {
                this.ViewActionCall(ViewEventType.UpdateParams,
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
            if (this.mainForm.InvokeRequired)
            {
                this.mainForm.BeginInvoke(new Action<RKResults, string>(this.DrawResult), res, curve);
            }
            else
            {
                this.mainForm.DrawResult(res, curve);
            }
        }

        #endregion
    }
}
