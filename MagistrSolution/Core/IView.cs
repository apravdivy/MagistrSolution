using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common;
using Common.Mathematic;
using Common.Task;
using ZedGraph;

namespace Core
{
    public sealed class ChartInfo
    {
        public string Title { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
    }

    public interface IView
    {
        event Func<ViewEventType, ViewEventArgs, object> OnViewAction;

        void ShowMainForm();

        void DrawPoint(double p, List<double> val, string curveName);

        void DrawPoint2(double p, List<double> list, string curveName);

        void SendSolvingResultType1(RKResults res, IFunctionExecuter fe);

        void DrawPhasePortret(double p, List<double> list, string curveName);

        void SendSolvingResultType2(RKResults res, IFunctionExecuter fe);

        void SendSolvingResultType2Mass(Dictionary<string, RKResults> results, IFunctionExecuter fe);

        void ConfigGraphPane(ZedGraphControl zgc, Form f, PaneLayout pl, List<ChartInfo> chInfo);

        void UpdateAlgorithmParameters(string t0, string t1, string y0, string y00, string y01, string rkN, string randN,
                                       string z1min, string z1max, string z2min, string z2max);

        TaskCollection loadTaskCollection();

        void SaveTaskCollection(TaskCollection tc);

        void DrawResult(RKResults res, string curve);
    }
}