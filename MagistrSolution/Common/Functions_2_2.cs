using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Mathematic;
using Common.Task;

namespace Common
{
    public enum FuncType
    {
        Main,
        Second
    }

    public enum SecondFuncType
    {
        Liner = 0,
        Square = 1,
        PeriodT = 2,
        PeriodZ = 3
    }

    public enum MainFuncSetCount
    {
        Two = 0,
        Three = 1,
        Four = 2,
        Five = 3
    }

    public class FAtrAttribute : Attribute
    {
        public int PCount;
        public string funcName;
        public FuncType funcType;

        public FAtrAttribute(FuncType t)
        {
            funcType = t;
            //this.funcName = name;
        }
    }

    public sealed class Functions_2_2
    {
        public Functions_2_2(string mainFuncName, string secFuncName)
        {
            MainFuncName = mainFuncName;
            SecFuncName = secFuncName;
        }

        public string MainFuncName { get; private set; }
        public string SecFuncName { get; private set; }

        private double CallSecFunc(string name, double t, Vector y, TaskParameter tp)
        {
            object res = GetType().InvokeMember(SecFuncName, BindingFlags.Public
                                                             | BindingFlags.InvokeMethod
                                                             | BindingFlags.Instance, null, this,
                                                new object[] {t, y, tp});
            return (double) res;
        }


        [FAtr(FuncType.Main)]
        public Vector MF1(double t, Vector v, TaskParameters p)
        {
            var res = new Vector(1);
            if (p.Gamma is List<double>)
            {
                var pp = new List<double>();
                (p.Gamma as List<double>).ForEach(x => pp.Add(x));

                pp.Insert(0, double.MinValue);
                pp.Add(double.MaxValue);

                //double a = double.MinValue;
                double b = pp[0];

                for (int i = 0; i < pp.Count - 1; i++)
                {
                    if (v[0] > pp[i] && v[0] <= pp[i + 1])
                    {
                        res[0] = CallSecFunc(SecFuncName, t, v, p[i]);
                    }
                    else
                        continue;
                }
            }
            else if (p.Gamma is Dictionary<double, int>)
            {
                try
                {
                    var tt = p.Gamma as Dictionary<double, int>;
                    //int i = -1;
                    double index = tt.Keys.ToList().Find(x => Math.Abs(x - t) < 0.01);
                    res[0] = CallSecFunc(SecFuncName, t, v, p[tt[index]]);
                }
                catch (Exception)
                {
                    res[0] = 1;
                }
            }
            else
                throw new NotImplementedException("not impl yet");
            return res;
        }


        [FAtr(FuncType.Main)]
        public Vector MF2(double t, Vector v, TaskParameters p)
        {
            var res = new Vector(2);
            res[0] = v[1];
            if (p.Gamma is List<double>)
            {
                var pp = new List<double>();
                (p.Gamma as List<double>).ForEach(x => pp.Add(x));

                pp.Insert(0, double.MinValue);
                pp.Add(double.MaxValue);

                //double a = double.MinValue;
                double b = pp[0];

                for (int i = 0; i < pp.Count - 1; i++)
                {
                    if (v[0] > pp[i] && v[0] <= pp[i + 1])
                    {
                        res[1] = CallSecFunc(SecFuncName, t, v, p[i]);
                    }
                    else
                        continue;
                }
            }
            else if (p.Gamma is Dictionary<double, int>)
            {
                try
                {
                    var tt = p.Gamma as Dictionary<double, int>;
                    //int i = -1;
                    double index = tt.Keys.ToList().Find(x => Math.Abs(x - t) < 0.01);
                    res[1] = CallSecFunc(SecFuncName, t, v, p[tt[index]]);
                }
                catch
                {
                    res[1] = 1;
                }
            }
            else
                throw new NotImplementedException("not impl yet");
            return res;
        }

        [FAtr(FuncType.Second, PCount = 3, funcName = "Linear [az+bt]")]
        public double Linear(double t, Vector y, TaskParameter tp)
        {
            return y.Count == 1
                       ? tp.Param[0]*y[0] + tp.Param[1]*t + tp.Param[2]
                       : tp.Param[0]*y[0] + tp.Param[1]*y[1] + tp.Param[2]*t;
        }

        [FAtr(FuncType.Second, PCount = 3, funcName = "Square [a0*(z1-a1)(z1-a1)+a2*t]")]
        public double Square(double t, Vector y, TaskParameter tp)
        {
            //return tp.Param[0] * y[0] * y[0] + tp.Param[1] * y[0] + tp.Param[2] * t;
            return tp.Param[0]*Math.Pow(y[0] - tp.Param[1], 2) + tp.Param[2]*t;
        }

        [FAtr(FuncType.Second, PCount = 2, funcName = "PeriodT [a0*sin(a1*t)]")]
        public double PeriodT(double t, Vector y, TaskParameter tp)
        {
            return tp.A*Math.Sin(tp.B*t);
        }

        [FAtr(FuncType.Second, PCount = 2, funcName = "PeriodZ [a0*sin(a1*z1)]")]
        public double PeriodZ(double t, Vector y, TaskParameter tp)
        {
            return tp.A*Math.Sin(tp.B*y[0]);
        }
    }
}