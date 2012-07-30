using System;
using System.Collections.Generic;
using Common;
using Common.Mathematic;

namespace Core
{
    public class Controler
    {
        private readonly IView view;
        private bool alive;
        private int randN;

        private int rkN = 50;
        //private int numMet = 0;

        private double t0;
        private double t1;
        private double y0;
        private double y00;
        private double y01;
        //private double delta;

        private double z1max;
        private double z1min;
        private double z2max;
        private double z2min;

        public Controler(IView view)
        {
            this.view = view;
            this.view.OnViewAction += view_ViewAction;
        }

        public bool Alive
        {
            get { return alive; }
        }

        public void Start()
        {
            alive = true;
            view.ShowMainForm();
        }


        private object view_ViewAction(ViewEventType type, ViewEventArgs args)
        {
            switch (type)
            {
                case ViewEventType.Exit:
                    {
                        alive = false;
                        break;
                    }
                case ViewEventType.StartSolving1:
                    {
                        string fname = args.Parameters[0].ToString();
                        string curveName = args.Parameters[1].ToString();
                        var fe = new FunctionExecuter(typeof (Functions), fname);
                        var r = new Random();
                        double y0 = z1min + r.NextDouble()*(z1max - z1min);
                        var rk = new RKVectorForm(fe, curveName, t0, t1, new Vector(1, y0));
                        rk.OnResultGenerated += rk_OnResultGenerated;
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.StartSolving2:
                    {
                        string fname = args.Parameters[0].ToString();
                        var curveNames = args.Parameters[1] as string[,];
                        var fe = new FunctionExecuter(typeof (Functions), fname);

                        double h1 = (z1max - z1min)/(randN);
                        double h2 = (z2max - z2min)/(randN);
                        var results = new Dictionary<string, RKResults>();
                        for (int i = 0; i < randN; i++)
                        {
                            for (int j = 0; j < randN; j++)
                            {
                                double z00 = z1min + i*h1;
                                double z01 = z2min + j*h2;
                                var rk = new RKVectorForm(fe, curveNames[i, j], t0, t1, new Vector(2, z00, z01));
                                rk.OnSolvingDone += rk_OnSolvingDone;
                                RKResults res = rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                                results.Add(curveNames[i, j], res);
                            }
                        }
                        break;
                    }
                case ViewEventType.SolvePodhod2Type1:
                    {
                        string fname = args.Parameters[0].ToString();
                        string curveName = args.Parameters[1].ToString();
                        var fe = new FunctionExecuter(typeof (Functions), fname);

                        var rk = new RKVectorForm(fe, curveName, t0, t1, new Vector(1, y0));

                        rk.OnResultGenerated += rk_OnResultGenerated;
                        rk.OnSolvingDone += rk_OnSolvingDoneType1;
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.SolovePodhod2Type2:
                    {
                        string fname = args.Parameters[0].ToString();
                        string curveName = args.Parameters[1].ToString();
                        var fe = new FunctionExecuter(typeof (Functions), fname);

                        var rk = new RKVectorForm(fe, curveName, t0, t1, new Vector(2, y00, y01));

                        rk.OnResultGenerated += rk_OnResGenForType2;
                        rk.OnSolvingDone += rk_OnSolvingDoneType2;
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.SolovePodhod2Type2Mass:
                    {
                        string fname = args.Parameters[0].ToString();
                        var curveNames = args.Parameters[1] as string[,];
                        var fe = new FunctionExecuter(typeof (Functions), fname);

                        double h1 = (z1max - z1min)/(randN);
                        double h2 = (z2max - z2min)/(randN);
                        var results = new Dictionary<string, RKResults>();
                        for (int i = 0; i < randN; i++)
                        {
                            for (int j = 0; j < randN; j++)
                            {
                                double z00 = z1min + i*h1;
                                double z01 = z2min + j*h2;
                                var rk = new RKVectorForm(fe, curveNames[i, j], t0, t1, new Vector(2, z00, z01));
                                //rk.OnResultGenerated += new RKResultGeneratedDelegate(rk_OnResGenForType2);
                                //rk.OnSolvingDone += new RKSolvingDoneDelegate(rk_OnSolvingDoneType2);
                                RKResults res = rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                                results.Add(curveNames[i, j], res);
                            }
                        }

                        view.SendSolvingResultType2Mass(results, fe);

                        break;
                    }
                case ViewEventType.UpdateParams:
                    {
                        t0 = (double) args.Parameters[0];
                        t1 = (double) args.Parameters[1];
                        y0 = (double) args.Parameters[2];
                        y00 = (double) args.Parameters[3];
                        y01 = (double) args.Parameters[4];

                        rkN = Convert.ToInt32(args.Parameters[5]);
                        randN = Convert.ToInt32(args.Parameters[6]);
                        z1min = (double) args.Parameters[7];
                        z1max = (double) args.Parameters[8];
                        z2min = (double) args.Parameters[9];
                        z2max = (double) args.Parameters[10];
                        break;
                    }
            }
            return null;
        }

        private void rk_OnSolvingDone(RKResults res, string curve, IFunctionExecuter fe)
        {
            view.DrawResult(res, curve);
        }

        private void rk_OnSolvingDoneType1(RKResults res, string c, IFunctionExecuter fe)
        {
            view.SendSolvingResultType1(res, fe);
        }

        private void rk_OnSolvingDoneType2(RKResults res, string c, IFunctionExecuter fe)
        {
            view.SendSolvingResultType2(res, fe);
        }


        private void rk_OnResultGenerated(RKResult res, string curveName)
        {
            view.DrawPoint(res.X, res.Y.GetValues(), curveName);
        }

        private void rk_OnResultGenerated2(RKResult res, string curveName)
        {
            view.DrawPoint2(res.X, res.Y.GetValues(), curveName);
        }

        private void rk_OnResGenForType2(RKResult res, string curveName)
        {
            view.DrawPhasePortret(res.X, res.Y.GetValues(), curveName);
        }
    }
}