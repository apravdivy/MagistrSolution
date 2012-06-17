using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Common.Mathematic;
using System.Reflection;

namespace Core
{
    public class Controler
    {
        private IView view;
        private bool alive;
        public bool Alive
        {
            get { return alive; }
        }

        private int rkN = 50;
        //private int numMet = 0;

        private double t0;
        private double t1;
        private double y0;
        private double y00;
        private double y01;
        //private double delta;

        private int randN;
        private double z1min;
        private double z1max;
        private double z2min;
        private double z2max;

        public Controler(IView view)
        {
            this.view = view;
            this.view.ViewAction += new Func<ViewEventType, ViewEventArgs,object>(view_ViewAction);
        }

        public void Start()
        {
            this.alive = true;
            this.view.ShowMainForm();
        }



        private object view_ViewAction(ViewEventType type, ViewEventArgs args)
        {
            switch (type)
            {
                case ViewEventType.Exit:
                    {
                        this.alive = false;
                        break;
                    }
                case ViewEventType.StartSolving1:
                    {
                        string fname = args.Parameters[0].ToString();
                        string curveName = args.Parameters[1].ToString();
                        FunctionExecuter fe = new FunctionExecuter(typeof(Functions), fname);
                        Random r = new Random();
                        double y0 = this.z1min + r.NextDouble() * (this.z1max - this.z1min);
                        RKVectorForm rk = new RKVectorForm(fe, curveName, this.t0, this.t1, new Vector(1, y0));
                        rk.OnResultGenerated += new Action<RKResult, string>(rk_OnResultGenerated);
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.StartSolving2:
                    {
                        string fname = args.Parameters[0].ToString();
                        string[,] curveNames = args.Parameters[1] as string[,];
                        FunctionExecuter fe = new FunctionExecuter(typeof(Functions), fname);

                        double h1 = (this.z1max - this.z1min) / (this.randN);
                        double h2 = (this.z2max - this.z2min) / (this.randN);
                        Dictionary<string, RKResults> results = new Dictionary<string, RKResults>();
                        for (int i = 0; i < this.randN; i++)
                        {
                            for (int j = 0; j < this.randN; j++)
                            {
                                double z00 = this.z1min + i * h1;
                                double z01 = this.z2min + j * h2;
                                RKVectorForm rk = new RKVectorForm(fe, curveNames[i, j], this.t0, this.t1, new Vector(2, z00, z01));
                                rk.OnSolvingDone += new Action<RKResults, string, IFunctionExecuter>(rk_OnSolvingDone);
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
                        FunctionExecuter fe = new FunctionExecuter(typeof(Functions), fname);

                        RKVectorForm rk = new RKVectorForm(fe, curveName, this.t0, this.t1, new Vector(1, this.y0));

                        rk.OnResultGenerated += new Action<RKResult, string>(rk_OnResultGenerated);
                        rk.OnSolvingDone += new Action<RKResults, string, IFunctionExecuter>(rk_OnSolvingDoneType1);
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.SolovePodhod2Type2:
                    {
                        string fname = args.Parameters[0].ToString();
                        string curveName = args.Parameters[1].ToString();
                        FunctionExecuter fe = new FunctionExecuter(typeof(Functions), fname);

                        RKVectorForm rk = new RKVectorForm(fe, curveName, this.t0, this.t1, new Vector(2, this.y00, this.y01));

                        rk.OnResultGenerated += new Action<RKResult, string>(rk_OnResGenForType2);
                        rk.OnSolvingDone += new Action<RKResults, string, IFunctionExecuter>(rk_OnSolvingDoneType2);
                        rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                        break;
                    }
                case ViewEventType.SolovePodhod2Type2Mass:
                    {
                        string fname = args.Parameters[0].ToString();
                        string[,] curveNames = args.Parameters[1] as string[,];
                        FunctionExecuter fe = new FunctionExecuter(typeof(Functions), fname);

                        double h1 = (this.z1max - this.z1min) / (this.randN);
                        double h2 = (this.z2max - this.z2min) / (this.randN);
                        Dictionary<string, RKResults> results = new Dictionary<string, RKResults>();
                        for (int i = 0; i < this.randN; i++)
                        {
                            for (int j = 0; j < this.randN; j++)
                            {
                                double z00 = this.z1min + i * h1;
                                double z01 = this.z2min + j * h2;
                                RKVectorForm rk = new RKVectorForm(fe, curveNames[i, j], this.t0, this.t1, new Vector(2, z00, z01));
                                //rk.OnResultGenerated += new RKResultGeneratedDelegate(rk_OnResGenForType2);
                                //rk.OnSolvingDone += new RKSolvingDoneDelegate(rk_OnSolvingDoneType2);
                                RKResults res = rk.SolveWithConstH(rkN, RKMetodType.RK4_1);
                                results.Add(curveNames[i, j], res);
                            }
                        }

                        this.view.SendSolvingResultType2Mass(results, fe);

                        break;
                    }
                case ViewEventType.UpdateParams:
                    {
                        this.t0 = (double)args.Parameters[0];
                        this.t1 = (double)args.Parameters[1];
                        this.y0 = (double)args.Parameters[2];
                        this.y00 = (double)args.Parameters[3];
                        this.y01 = (double)args.Parameters[4];

                        this.rkN = Convert.ToInt32(args.Parameters[5]);
                        this.randN = Convert.ToInt32(args.Parameters[6]);
                        this.z1min = (double)args.Parameters[7];
                        this.z1max = (double)args.Parameters[8];
                        this.z2min = (double)args.Parameters[9];
                        this.z2max = (double)args.Parameters[10];
                        break;
                    }
            }
            return null;
        }

        private void rk_OnSolvingDone(RKResults res, string curve, IFunctionExecuter fe)
        {
            this.view.DrawResult(res, curve);
        }

        private void rk_OnSolvingDoneType1(RKResults res, string c, IFunctionExecuter fe)
        {
            this.view.SendSolvingResultType1(res, fe);
        }

        private void rk_OnSolvingDoneType2(RKResults res, string c, IFunctionExecuter fe)
        {
            this.view.SendSolvingResultType2(res, fe);
        }


        private void rk_OnResultGenerated(RKResult res, string curveName)
        {
            this.view.DrawPoint(res.X, res.Y.GetValues(), curveName);
        }

        private void rk_OnResultGenerated2(RKResult res, string curveName)
        {
            this.view.DrawPoint2(res.X, res.Y.GetValues(), curveName);

        }

        private void rk_OnResGenForType2(RKResult res, string curveName)
        {
            this.view.DrawPhasePortret(res.X, res.Y.GetValues(), curveName);
        }


    }
}
