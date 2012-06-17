using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Common.Mathematic;

namespace Common
{
    public sealed class FunctionExecuter : IFunctionExecuter
    {
        private Type classType;
        private string methodName;
        private string[] fnames;

        public int SetCount
        {
            get { return fnames.Length; }
        }

        public Vector AbstractFunction(double t, Vector y)
        {
            object res = this.classType.InvokeMember(methodName, BindingFlags.Public
                                     | BindingFlags.InvokeMethod
                                     | BindingFlags.Instance | BindingFlags.Static, null, null, new object[] { t, y });
            return res as Vector;
        }

        public double FunctionSet(double t, Vector y, int index)
        {
            object res = this.classType.InvokeMember(fnames[index], BindingFlags.Public
                                     | BindingFlags.InvokeMethod
                                     | BindingFlags.Instance | BindingFlags.Static, null, null, new object[] { t, y });
            return (double)res;
        }

        public FunctionExecuter(Type type, string method)
        {
            this.classType = type;
            this.methodName = method;

            MethodInfo mi = type.GetMethod(this.methodName);
            object[] atr = mi.GetCustomAttributes(false);
            if (atr.Length > 0)
            {
                FuncDescrAttribute fa = atr[0] as FuncDescrAttribute;
                this.fnames = fa.Methods;
            }
        }


    }
}
