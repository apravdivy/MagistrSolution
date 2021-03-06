﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Task;

namespace Common.Mathematic
{
    public sealed class RAlgSolver
    {
        #region Delegates

        public delegate void RAlgFunctionDelegate(ref double f_1, ref double[] x_1, ref double[] g_1);

        #endregion

        private const int maxn = 40; //max kolichestvo peremennih
        private readonly Func<TaskParameters, double> calcFunct;
        private readonly double[] dz;
        private readonly RKResults res;
        private readonly Vector startP;
        private readonly TaskParameters tp;
        private readonly TaskWorker tw;
        public RAlgFunctionDelegate FUNCT;
        private int alp = 3;
        public double alpha;
        public int c = -1; //kolichestvo peremennih
        public double f;
        private double[] g = new double[maxn];
        private double[] g2 = new double[maxn];
        private int h = 1;
        private int ii = -1;
        private int iis = 1;
        private int intt = 1;
        public int[] itab;
        private double[] kof_1 = new double[maxn];
        private int ks = 1000;
        private int[,] lambda;
        private int nh = 3;
        private double q1 = 0.9;
        private double q2 = 1.1;
        private double sg = 0.00001;
        private double sx = 0.00001;
        //private int m1 = 11;
        //private int m2 = 12;
        //private int mr = 121;
        private double[] t_1 = new double[maxn];
        public double[] x = new double[maxn];

        public RAlgSolver()
        {
            c = 3;
            x[0] = 0.16;
            x[1] = 100;
            x[2] = 300;
        }

        public RAlgSolver(int count)
        {
            c = count;
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                x[i] = r.NextDouble();    
            }
        }

        private RAlgSolver(TaskWorker tw, RKResults res, List<List<double>> startParameters)
        {
            this.tw = tw;
            this.res = res;

            itab = new int[res.Count - 1];

            if (res[0].Y.Count == 2)
            {
                var dz1 = new double[res.Count - 1];
                for (int i = 1; i < res.Count; i++)
                {
                    dz1[i - 1] = (res[i].Y[0] - res[i - 1].Y[0]) / (res[i].X - res[i - 1].X);
                }
                var dz2 = new double[res.Count - 2];
                for (int i = 1; i < dz1.Length; i++)
                {
                    dz2[i - 1] = (dz1[i] - dz1[i - 1]) / (res[i].X - res[i - 1].X);
                }
                dz = dz2;
            }

            var list = new List<TaskParameter>();

            int k = 0;
            for (int i = 0; i < startParameters.Count; i++)
            {
                var tp1 = new TaskParameter(startParameters[i].Count);
                for (int j = 0; j < startParameters[i].Count; j++)
                {
                    x[k] = startParameters[i][j];
                    tp1.Param[j] = x[k];
                    k++;
                }
                list.Add(tp1);
            }
            c = k;
            tp = new TaskParameters(list, double.MinValue);
        }


        public RAlgSolver(TaskWorker tw, RKResults res, List<List<double>> startParameters, double[] dz)
            : this(tw, res, startParameters)
        {
            this.dz = dz;
        }

        public RAlgSolver(TaskWorker tw, RKResults res, List<List<double>> startParameters, Vector startP, int method)
            : this(tw, res, startParameters)
        {
            this.startP = startP;
            if (this.startP.Count == 1)
            {
                calcFunct += CalcFunct1;
            }
            else
            {
                if (method == 0)
                {
                    calcFunct += CalcFunct2;
                }
                else
                {
                    calcFunct += CalcFunct2_1;
                }
            }
        }

        private double sqr(double val)
        {
            return Math.Pow(val, 2);
        }

        //public void FUNCT2(double f_1, ref double[] x_1, ref double[] g_1)//st-nomer stroki
        //{

        //    this.tp[0].A = x[0];
        //    this.tp[0].B = x[1];
        //    this.tp[1].A = x[2];
        //    this.tp[1].B = x[3];

        //    f_1 = this.Integral(this.tp);

        //    double delta = 0.001;

        //    TaskParameters tp1 = tp.Clone() as TaskParameters;
        //    tp1[0].A += delta;
        //    double d1 = this.Integral(tp1);
        //    g_1[0] = (d1 - f_1) / delta;

        //    TaskParameters tp2 = tp.Clone() as TaskParameters;
        //    tp2[0].B += delta;
        //    double d2 = this.Integral(tp2);
        //    g_1[1] = (d2 - f_1) / delta;

        //    TaskParameters tp3 = tp.Clone() as TaskParameters;
        //    tp3[1].A += delta;
        //    double d3 = this.Integral(tp3);
        //    g_1[2] = (d3 - f_1) / delta;

        //    TaskParameters tp4 = tp.Clone() as TaskParameters;
        //    tp4[1].B += delta;
        //    double d4 = this.Integral(tp4);
        //    g_1[3] = (d4 - f_1) / delta;


        //    //g_1[0] = 2 * x_1[0];
        //    //g_1[1] = 2 * x_1[1];
        //    //g_1[2] = 2 * x_1[2];

        //}

        public void FUNCT3(ref double f_1, ref double[] x_1, ref double[] g_1) //st-nomer stroki
        {
            int k = 0;

            for (int i = 0; i < tp.Count; i++)
            {
                for (int j = 0; j < tp[i].Param.Length; j++)
                {
                    tp[i].Param[j] = x[k];
                    k++;
                }
            }

            f_1 = Integral(tp);

            double delta = 0.001;
            k = 0;
            for (int i = 0; i < tp.Count; i++)
            {
                for (int j = 0; j < tp[i].Param.Length; j++)
                {
                    var tp1 = tp.Clone() as TaskParameters;
                    tp1[i].Param[j] += delta;
                    double val = Integral(tp1);
                    g_1[k] = (val - f_1) / delta;
                    k++;
                }
            }
        }

        private Vector RK4_1(double x, Vector y, double h, int j, TaskParameters taskP)
        {
            var res = new Vector(2);
            Vector f0, f1, f2, f3;

            f0 = h * ff(x, y, j, taskP);
            f1 = h * ff(x + h / 2, y + f0 / 2, j, taskP);
            f2 = h * ff(x + h / 2, y + f1 / 2, j, taskP);
            f3 = h * ff(x + h, y + f2, j, taskP);
            res = y + (f0 + 2 * f1 + 2 * f2 + f3) / 6;

            return res;
        }

        private Vector ff(double t, Vector y, int j, TaskParameters taskP)
        {
            return new Vector(2, y[1], tw.FunctionSet(t, y, j, taskP));
        }

        private double CalcFunct1(TaskParameters taskP)
        {
            double f = 0;
            var val = new List<double>();
            var v = new List<Vector>();
            double h = res.GetH();
            var rk = new Vector[res.Count - 1];
            rk[0] = new Vector(1, startP[0]);
            itab[0] = 1;
            double t = res[0].X;
            for (int i = 1; i < res.Count - 1; i++)
            {
                val.Clear();
                v.Clear();
                for (int j = 0; j < tw.SetCount; j++)
                {
                    double x1 = rk[i - 1][0] + tw.FunctionSet(t, rk[i - 1], j, taskP) * h;
                    v.Add(new Vector(1, x1));
                    val.Add(Math.Abs(x1 - res[i].Y[0]));
                }
                double min = val.Min();
                int index = val.IndexOf(min);
                rk[i] = v[index];
                itab[i] = index + 1;
                f += min * min * h;
                t = t + h;
            }

            return f;
        }

        private double CalcFunct2(TaskParameters taskP)
        {
            double f = 0;
            var val = new List<double>();
            var min1List = new List<double>();
            var min2List = new List<double>();
            var v = new List<Vector>();
            double h = res.GetH();
            var rk = new Vector[res.Count - 1];
            rk[0] = new Vector(2, startP[0], startP[1]);
            itab[0] = 1;
            double t = res[0].X;
            for (int i = 1; i < res.Count - 2; i++)
            {
                val.Clear();
                min1List.Clear();
                v.Clear();
                for (int j = 0; j < tw.SetCount; j++)
                {
                    //double x2 = rk[i - 1][1] + this.tw.FunctionSet(t, rk[i - 1], j, taskP) * h;
                    //double x1 = rk[i - 1][0] + x2 * h;
                    //Vector vv = new Vector(2, x1, x2);

                    Vector vv = RK4_1(t, rk[i - 1], h, j, taskP);
                    v.Add(vv);
                    double min1 = Math.Abs(vv[0] - res[i].Y[0]);
                    double min2 = Math.Abs(vv[1] - dz[i]);
                    val.Add(min1 + min2);
                    min1List.Add(min1);
                    min2List.Add(min2);

                    //if (i < this.dz.Length)
                    //{
                    //    val2.Add(Math.Abs(vv[1] - this.dz[i]));
                    //}
                }
                double min = val.Min();
                int index = val.IndexOf(min);
                rk[i] = v[index];
                itab[i] = index + 1;
                //f += min * min * h;
                f += Math.Pow(min1List[index], 2) + Math.Pow(min2List[index], 2) * h;
                t = t + h;
            }

            return f;
        }

        private double CalcFunct2_1(TaskParameters taskP)
        {
            double f = 0;
            var val = new List<double>();
            var min1List = new List<double>();
            //List<double> min2List = new List<double>();
            var v = new List<Vector>();
            double h = res.GetH();
            var rk = new Vector[res.Count - 1];
            rk[0] = new Vector(2, startP[0], startP[1]);
            itab[0] = 1;
            double t = res[0].X;
            for (int i = 1; i < res.Count - 2; i++)
            {
                val.Clear();
                min1List.Clear();
                v.Clear();
                for (int j = 0; j < tw.SetCount; j++)
                {
                    Vector vv = RK4_1(t, rk[i - 1], h, j, taskP);
                    v.Add(vv);
                    double min1 = Math.Abs(vv[0] - res[i].Y[0]);
                    //double min2 = Math.Abs(vv[1] - this.dz[i]);
                    val.Add(min1);
                    min1List.Add(min1);
                    //min2List.Add(min2);

                    //if (i < this.dz.Length)
                    //{
                    //    val2.Add(Math.Abs(vv[1] - this.dz[i]));
                    //}
                }
                double min = val.Min();
                int index = val.IndexOf(min);
                rk[i] = v[index];
                itab[i] = index + 1;
                //f += min * min * h;
                f += (Math.Pow(min1List[index], 2) + alpha * (v[index][0] * v[index][0] + v[index][1] * v[index][1])) * h;
                t = t + h;
            }

            return f;
        }

        public void FUNCT4(ref double f_1, ref double[] x_1, ref double[] g_1) //st-nomer stroki
        {
            int k = 0;

            for (int i = 0; i < tp.Count; i++)
            {
                for (int j = 0; j < tp[i].Param.Length; j++)
                {
                    tp[i].Param[j] = x[k];
                    k++;
                }
            }

            f_1 = calcFunct(tp);

            double delta = 0.0001;
            k = 0;
            for (int i = 0; i < tp.Count; i++)
            {
                for (int j = 0; j < tp[i].Param.Length; j++)
                {
                    var tp1 = tp.Clone() as TaskParameters;
                    tp1[i].Param[j] += delta;
                    double val = calcFunct(tp1);
                    g_1[k] = (val - f_1) / delta;
                    k++;
                }
            }
        }

        //public void FUNCT5(ref double f_1, ref double[] x_1, ref double[] g_1)//st-nomer stroki
        //{
        //    int k = 0;

        //    for (int i = 0; i < tp.Count; i++)
        //    {
        //        for (int j = 0; j < this.tp[i].Param.Length; j++)
        //        {
        //            this.tp[i].Param[j] = x[k];
        //            k++;
        //        }
        //    }

        //    f_1 = this.CalcFunct1(this.tp);

        //    double delta = 0.0001;
        //    k = 0;
        //    for (int i = 0; i < tp.Count; i++)
        //    {
        //        for (int j = 0; j < tp[i].Param.Length; j++)
        //        {
        //            TaskParameters tp1 = tp.Clone() as TaskParameters;
        //            tp1[i].Param[j] += delta;
        //            double val = this.CalcFunct1(tp1);
        //            g_1[k] = (val - f_1) / delta;
        //            k++;
        //        }
        //    }
        //}


        /// <summary>
        /// R-алгоритм
        /// </summary>
        public double[] R_Algorithm()
        {
            int k, k1, k2, ll;
            double sx1, sg1, h1, w, d, d1, dd;
            var g1 = new double[maxn];
            var z = new double[maxn];

            var hh = new double[maxn, maxn];

            k = 0;
            h1 = ii * h;
            sx1 = sqr(sx);
            sg1 = sqr(sg);
            w = 1 / sqr(alp) - 1;
            ll = k + intt;
        m_100:
            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < c; j++)
                {
                    hh[i, j] = 0;
                    hh[i, i] = 1;
                }
            }
            FUNCT(ref f, ref x, ref g);
            d = 0;
            for (int i = 0; i < c; i++)
            {
                d = d + sqr(g[i]);
            }
            if (d < sg1)
                return x;
            for (int i = 0; i < c; i++)
            {
                g1[i] = g[i];
                z[i] = x[i];
            }
        //Общий шаг алгоритма}
        m_101:
            k = k + 1;
            for (int i = 0; i < c; i++)
                g2[i] = g1[i] - g[i];
            d = 0;
            for (int i = 0; i < c; i++)
                d = d + sqr(g2[i]);
            if (d > Math.Pow(10, -18))
            {
                for (int i = 0; i < c; i++)
                {
                    g1[i] = 0;
                    for (int j = 0; j < c; j++)
                    {
                        g1[i] = g1[i] + hh[i, j] * g2[j];
                    }
                }
                d = 0;
                for (int i = 0; i < c; i++)
                {
                    d = d + g2[i] * g1[i];
                }
                d = w / d;
                for (int i = 0; i < c; i++)
                {
                    d1 = d * g1[i];
                    for (int j = 0; j < c; j++)
                    {
                        hh[i, j] = hh[i, j] + d1 * g1[j];
                    }
                }
            }
            for (int i = 0; i < c; i++)
            {
                g1[i] = g[i];
            }
            //вычисление направления движения
            for (int i = 0; i < c; i++)
            {
                g[i] = 0;
                for (int j = 0; j < c; j++)
                {
                    g[i] = g[i] + hh[i, j] * g1[j];
                }
            }
            d = 0;
            for (int i = 0; i < c; i++)
            {
                d = d + g1[i] * g[i];
            }
            if (d < Math.Pow(10, -18))
            {
                d = 0;
                for (int i = 0; i < c; i++)
                {
                    d = d + sqr(x[i] - z[i]);
                }
                d = Math.Sqrt(d);
                h1 = ii * d / nh;
                k = k - 1;
                goto m_100;
            }
            else
            {
                d = Math.Sqrt(d);
                for (int i = 0; i < c; i++)
                    g[i] = g[i] / d;
                for (int i = 0; i < c; i++)
                    z[i] = x[i];
                //Алгоритм одномерного спуска
                k1 = 0;
                k2 = 20;
            m_103:
                for (int i = 0; i < c; i++)
                    x[i] = x[i] + h1 * g[i];
                FUNCT(ref f, ref x, ref g2);


                k1 = k1 + 1;
                if (iis != 0 && k1 >= k2)
                {
                    k2 = k2 + 20;
                    //сообщение
                    Console.WriteLine("--");
                    //MessageDlg('Tююс•хэшх',mtInformation,[mbok],0);
                    return x;
                }
                if (k1 >= nh)
                    h1 = h1 * q2;
                d = 0;
                for (int i = 0; i < c; i++)
                    d = d + g[i] * g2[i];
                if (d > 0)
                    goto m_103;
                if (k1 <= 1)
                    h1 = h1 * q1;
                for (int i = 0; i < c; i++)
                    g[i] = g2[i];
                //печать промежуточной информации и проверка критерия останова
                if (ll == k)
                {
                    ll = ll + intt;
                    //печать
                }
                if (k >= ks)
                    return x;
                else
                {
                    dd = 0;
                    for (int i = 0; i < c; i++)
                        dd = dd + sqr(g[i]);
                    if (dd < sg1)
                        return x;
                    else
                    {
                        d = 0;
                        for (int i = 0; i < c; i++)
                            d = d + sqr(x[i] - z[i]);
                        if (d < sx1)
                            return x;
                        else
                        //корректировка матрицы
                        {
                            d1 = Math.Abs(h1) - Math.Pow(10, 10);
                            if (d1 <= 0)
                                goto m_101;
                            else
                            {
                                d = Math.Sqrt(d);
                                h1 = ii * d / nh;
                                goto m_100;
                            }
                        }
                    }
                }
            }
        }

        public double Integral(TaskParameters taskP)
        {
            lambda = new int[taskP.Count, dz.Length];
            itab = new int[dz.Length];

            var diffValues = new List<double>();
            int k;
            for (int i = 0; i < dz.Length; i++)
            {
                diffValues.Clear();
                for (int j = 0; j < tw.SetCount; j++)
                {
                    double d = Math.Abs(dz[i] - tw.FunctionSet(res[i].X, res[i].Y, j, taskP));
                    diffValues.Add(d);
                }
                k = diffValues.IndexOf(diffValues.Min());
                for (int j = 0; j < tw.SetCount; j++)
                {
                    lambda[j, i] = j == k ? 1 : 0;
                }
                itab[i] = k + 1;
            }

            double val = 0;
            for (int i = 0; i < dz.Length - 1; i++)
            {
                for (int j = 0; j < tw.SetCount; j++)
                {
                    val += lambda[j, i] * Math.Pow(dz[i] - tw.FunctionSet(res[i].X, res[i].Y, j, taskP), 2);
                }
            }
            val *= res.GetH();
            return val;
        }
    }
}