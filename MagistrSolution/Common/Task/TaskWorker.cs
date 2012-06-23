using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Common.Mathematic;
using Common.Task;

namespace Common.Task
{
    public sealed class TaskWorker : IFunctionExecuter
    {

        public TaskParameters TaskParams { get; private set; }
        public string MainFunctionName { get; private set; }
        public string SecondFunctionName { get; private set; }
        private Type classType;

        private object target;

        public TaskWorker(TaskParameters p, Type t, string mf, string sf,object target)
        {
            this.TaskParams = p;
            this.MainFunctionName = mf;
            this.SecondFunctionName = sf;
            this.classType = t;
            this.target = target;
        }


        public Vector AbstractFunction(double t, Vector y)
        {
            object res = this.classType.InvokeMember(MainFunctionName, BindingFlags.Public
                                      | BindingFlags.InvokeMethod
                                      | BindingFlags.Instance | BindingFlags.Static, null, target,
                                      new object[] { t, y, this.TaskParams });
            return res as Vector;
        }

        public Vector AbstractFunction(double t, Vector y,TaskParameters tps)
        {
            object res = this.classType.InvokeMember(MainFunctionName, BindingFlags.Public
                                      | BindingFlags.InvokeMethod
                                      | BindingFlags.Instance | BindingFlags.Static, null, target,
                                      new object[] { t, y, tps });
            return res as Vector;
        }

        public double FunctionSet(double t, Vector y, int index)
        {
            object res = this.classType.InvokeMember(this.SecondFunctionName, BindingFlags.Public
                                               | BindingFlags.InvokeMethod
                                               | BindingFlags.Instance | BindingFlags.Static, null, target,
                                               new object[] { t, y, this.TaskParams[index] });
            return (double)res;
        }
        public double FunctionSet(double t, Vector y, int index,TaskParameters tp)
        {
            object res = this.classType.InvokeMember(this.SecondFunctionName, BindingFlags.Public
                                               | BindingFlags.InvokeMethod
                                               | BindingFlags.Instance | BindingFlags.Static, null, target,
                                               new object[] { t, y, tp[index] });
            return (double)res;
        }

        public int SetCount
        {
            get { return this.TaskParams.Count; }
        }


        
    }


  
}
