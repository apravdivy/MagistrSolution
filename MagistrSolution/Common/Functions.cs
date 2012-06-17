using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Mathematic;
using System.Reflection;

namespace Common
{
    public class Functions
    {
        public static double Alpha;
        public static double alpha = 5;

        #region 2
        [FuncDescr(2, Methods = new string[] { "Sf1_u1", "Sf1_u2" })]
        public static Vector SystemFunctionTwo1(double t, Vector y)
        {
            return new Vector(2, y[1], y[0] > 0 ? Sf1_u1(t, y) : Sf1_u2(t, y));
        }
        public static double Sf1_u1(double t, Vector y)
        {
            return 1;
        }
        public static double Sf1_u2(double t, Vector y)
        {
            return -1;
        }

        [FuncDescr(2, Methods = new string[] { "Sf2_u1", "Sf2_u2" })]
        public static Vector SystemFunctionTwo2(double t, Vector y)
        {
            return new Vector(2, y[1], y[0] > y[1] ? Sf2_u1(t, y) : Sf2_u2(t, y));
        }
        public static double Sf2_u1(double t, Vector y)
        {
            return 1;
        }
        public static double Sf2_u2(double t, Vector y)
        {
            return -1;
        }

        [FuncDescr(2, Methods = new string[] { "SystemFunctionTwo3_u1", "SystemFunctionTwo3_u2" })]
        public static Vector SystemFunctionTwo3(double t, Vector y)
        {
            Vector v = new Vector(2);
            v[0] = y[1];
            v[1] = y[0] > (0.1 * y[1] + 5) ? SystemFunctionTwo3_u1(t, y) : SystemFunctionTwo3_u2(t, y);
            return v;
        }
        public static double SystemFunctionTwo3_u1(double t, Vector v)
        {
            return 0.1 * t;
        }
        public static double SystemFunctionTwo3_u2(double t, Vector v)
        {
            return -0.1 * t + 0.32 * v[0];
        }
        #endregion
        #region 1
        [FuncDescr(1, Methods = new string[] { "SystemFunction_u1", "SystemFunction_u2", "SystemFunction_u3" })]
        public static Vector Example1(double t, Vector y)
        {
            Vector v = new Vector(1);
            if (t <= 0)
            {
                v[0] = SystemFunction_u1(t, y);
            }
            else
                if (t < 2)
                {
                    v[0] = SystemFunction_u2(t, y);
                }
                else
                {
                    v[0] = SystemFunction_u3(t, y);
                }

            return v;
        }
        public static double SystemFunction_u1(double t, Vector y)
        {
            return t;
        }
        public static double SystemFunction_u2(double t, Vector y)
        {
            return 1;
        }
        public static double SystemFunction_u3(double t, Vector y)
        {
            return 10;
        }



        [FuncDescr(1, Methods = new string[] { "F31_u1", "F31_u2", "F31_u3" })]
        public static Vector Example2(double t, Vector y)
        {
            Vector v = new Vector(1);
            if (y[0] < Functions.alpha)
            {
                v[0] = F31_u1(t, y);
            }
            else
            {
                if (y[0] < Functions.alpha + 2)
                {
                    v[0] = F31_u2(t, y);
                }
                else
                {
                    v[0] = F31_u3(t, y);
                }
            }
            return v;
        }
        public static double F31_u1(double t, Vector y)
        {
            return t;
        }
        public static double F31_u2(double t, Vector y)
        {
            return 0.5;// 0.5 * t * t + 1;
        }
        public static double F31_u3(double t, Vector y)
        {
            return -t;
        }

        [FuncDescr(1, Methods = new string[] { "ForCompare_u1", "ForCompare_u2" })]
        public static Vector Example3(double t, Vector y)
        {
            return new Vector(1, y[0] <= 5 ? ForCompare_u1(t, y) : ForCompare_u2(t, y));
        }
        public static double ForCompare_u1(double t, Vector y)
        {
            return y[0];
        }
        public static double ForCompare_u2(double t, Vector y)
        {
            return 0.25 * y[0];
        }

        [FuncDescr(1, Methods = new string[] { "F32_u1", "F32_u2", "F32_u3" })]
        public static Vector F32(double t, Vector y)
        {
            Vector v = new Vector(1);
            //v[0] = y[0] < Functions.alpha ? SimpleFunc1_u1(t, y) : SimpleFunc1_u2(t, y);
            if (y[0] < Functions.alpha)
            {
                v[0] = F32_u1(t, y);
            }
            else
            {
                if (y[0] < Functions.alpha + 5)
                {
                    v[0] = F32_u2(t, y);
                }
                else
                {
                    v[0] = F32_u3(t, y);
                }
            }
            return v;
        }
        public static double F32_u1(double t, Vector y)
        {
            return -1; //t* t;//System.Math.Sin(t);
        }
        public static double F32_u2(double t, Vector y)
        {
            return -0.3;
        }
        public static double F32_u3(double t, Vector y)
        {
            return -0.5;
        }


        // my version
        [FuncDescr(1, Methods = new string[] { "F33_u1", "F33_u2", "F33_u3" })]
        public static Vector F33(double t, Vector y)
        {
            Vector v = new Vector(1);
            //v[0] = y[0] < Functions.alpha ? SimpleFunc1_u1(t, y) : SimpleFunc1_u2(t, y);
            if (y[0] < 5)
            {
                v[0] = F33_u1(t, y);
            }
            else
            {
                if (y[0] < 7)
                {
                    v[0] = F33_u2(t, y);
                }
                else
                {
                    v[0] = F33_u3(t, y);
                }
            }
            return v;
        }
        public static double F33_u1(double t, Vector y)
        {
            return 0.03 * y[0] * System.Math.Sin(y[0] * t / 4);
        }
        public static double F33_u2(double t, Vector y)
        {
            return 0.05 * y[0] * y[0] + 1;
        }
        public static double F33_u3(double t, Vector y)
        {
            return -0.0001 * y[0] * y[0] * y[0];
        }
        // end my version

        [FuncDescr(1, Methods = new string[] { "SimpleFunc2_u1", "SimpleFunc2_u2" })]
        public static Vector Experimental(double t, Vector y)
        {
            Vector v = new Vector(1);
            v[0] = y[0] <= 2 ? SimpleFunc2_u1(t, y) : SimpleFunc2_u2(t, y);
            return v;
        }
        public static double SimpleFunc2_u1(double t, Vector y)
        {
            return 1;
        }
        public static double SimpleFunc2_u2(double t, Vector y)
        {
            return -1;
        }

        [FuncDescr(1, Methods = new string[] 
        { "Sf_10_u1", 
            "Sf_10_u2", 
            "Sf_10_u3", 
            "Sf_10_u4",
            "Sf_10_u5",
            "Sf_10_u6",
            "Sf_10_u7",
            "Sf_10_u8",
            "Sf_10_u9",
            "Sf_10_u10"
        })]
        public static Vector Sf_10(double t, Vector y)
        {
            Vector v = new Vector(1);
            if (y[0] < 1)
                v[0] = Sf_10_u1(t, y);
            else
                if (y[0] < 2)
                    v[0] = Sf_10_u2(t, y);
                else
                    if (y[0] < 4)
                        v[0] = Sf_10_u3(t, y);
                    else
                        if (y[0] < 7)
                            v[0] = Sf_10_u4(t, y);
                        else
                            if (y[0] < 11)
                                v[0] = Sf_10_u5(t, y);
                            else
                                if (y[0] < 13)
                                    v[0] = Sf_10_u6(t, y);
                                else
                                    if (y[0] < 14)
                                        v[0] = Sf_10_u7(t, y);
                                    else
                                        if (y[0] < 17)
                                            v[0] = Sf_10_u8(t, y);
                                        else
                                            if (y[0] < 20)
                                                v[0] = Sf_10_u9(t, y);
                                            else
                                                v[0] = Sf_10_u10(t, y);
            return v;
        }
        public static double Sf_10_u1(double t, Vector y)
        {
            return 6;
        }
        public static double Sf_10_u2(double t, Vector y)
        {
            return t;
        }
        public static double Sf_10_u3(double t, Vector y)
        {
            return 9;
        }
        public static double Sf_10_u4(double t, Vector y)
        {
            return t*t;
        }
        public static double Sf_10_u5(double t, Vector y)
        {
            return 1;
        }
        public static double Sf_10_u6(double t, Vector y)
        {
            return 7;
        }
        public static double Sf_10_u7(double t, Vector y)
        {
            return 0.1;
        }
        public static double Sf_10_u8(double t, Vector y)
        {
            return 4;
        }
        public static double Sf_10_u9(double t, Vector y)
        {
            return t+t*t;
        }
        public static double Sf_10_u10(double t, Vector y)
        {
            return 0.7;
        }

        [FuncDescr(1, Methods = new string[] { "sf4_u1", "sf4_u2", "sf4_u3" })]
        public static Vector ExpPresent(double t, Vector y)
        {
            Vector v = new Vector(1);
            if (y[0] < 5)
            {
                v[0] = sf4_u1(t, y);
            }
            else
            {
                if (y[0] >= 5 && y[0] <= 7)
                {
                    v[0] = sf4_u2(t, y);
                }
                else
                {
                    v[0] = sf4_u3(t, y);
                }
            }
            return v;
        }
        public static double sf4_u1(double t, Vector y)
        {
            return 0.03 * y[0] * Math.Sin(y[0] * t / 4);
        }
        public static double sf4_u2(double t, Vector y)
        {
            return 0.05 * y[0] * y[0] - 0.005 * t;
        }
        public static double sf4_u3(double t, Vector y)
        {
            return -0.0001 * Math.Pow(y[0], 3);
        }

        //public static Vector FunctionExample(double t, Vector z)
        //{
        //    return new Vector(1, 1 - 0.5 * Math.Sign(Alpha - z[0]));
        //}
        #endregion

        public static List<string> GetFunctionNames(int n)
        {
            List<string> res = new List<string>();

            Type type = typeof(Functions);

            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo m in methods)
            {
                object[] atr = m.GetCustomAttributes(typeof(FuncDescrAttribute), false);
                if (atr != null && atr.Length > 0)
                {
                    if ((atr[0] as FuncDescrAttribute).N == n)
                    {
                        res.Add(m.Name);
                    }
                }
            }
            return res;
        }

    }
}

