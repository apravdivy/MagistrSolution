using System;
using System.Reflection;
using Common.Mathematic;

namespace Common
{
    public sealed class FunctionExecuter : IFunctionExecuter
    {
        private readonly Type classType;
        private readonly string[] fnames;
        private readonly string methodName;

        public FunctionExecuter(Type type, string method)
        {
            classType = type;
            methodName = method;

            MethodInfo mi = type.GetMethod(methodName);
            object[] atr = mi.GetCustomAttributes(false);
            if (atr.Length > 0)
            {
                var fa = atr[0] as FuncDescrAttribute;
                fnames = fa.Methods;
            }
        }

        #region IFunctionExecuter Members

        public int SetCount
        {
            get { return fnames.Length; }
        }

        public Vector AbstractFunction(double t, Vector y)
        {
            object res = classType.InvokeMember(methodName, BindingFlags.Public
                                                            | BindingFlags.InvokeMethod
                                                            | BindingFlags.Instance | BindingFlags.Static, null, null,
                                                new object[] {t, y});
            return res as Vector;
        }

        public double FunctionSet(double t, Vector y, int index)
        {
            object res = classType.InvokeMember(fnames[index], BindingFlags.Public
                                                               | BindingFlags.InvokeMethod
                                                               | BindingFlags.Instance | BindingFlags.Static, null, null,
                                                new object[] {t, y});
            return (double) res;
        }

        #endregion
    }
}