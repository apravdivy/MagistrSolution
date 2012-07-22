using System;
using System.Reflection;
using Common.Mathematic;

namespace Common.Task
{
    public sealed class TaskWorker : IFunctionExecuter
    {
        private readonly Type classType;

        private readonly object target;

        public TaskWorker(TaskParameters p, Type t, string mf, string sf, object target)
        {
            TaskParams = p;
            MainFunctionName = mf;
            SecondFunctionName = sf;
            classType = t;
            this.target = target;
        }

        public TaskParameters TaskParams { get; private set; }
        public string MainFunctionName { get; private set; }
        public string SecondFunctionName { get; private set; }

        #region IFunctionExecuter Members

        public Vector AbstractFunction(double t, Vector y)
        {
            object res = classType.InvokeMember(MainFunctionName, BindingFlags.Public
                                                                  | BindingFlags.InvokeMethod
                                                                  | BindingFlags.Instance | BindingFlags.Static, null,
                                                target,
                                                new object[] {t, y, TaskParams});
            return res as Vector;
        }

        public double FunctionSet(double t, Vector y, int index)
        {
            object res = classType.InvokeMember(SecondFunctionName, BindingFlags.Public
                                                                    | BindingFlags.InvokeMethod
                                                                    | BindingFlags.Instance | BindingFlags.Static, null,
                                                target,
                                                new object[] {t, y, TaskParams[index]});
            return (double) res;
        }

        public int SetCount
        {
            get { return TaskParams.Count; }
        }

        #endregion

        public Vector AbstractFunction(double t, Vector y, TaskParameters tps)
        {
            object res = classType.InvokeMember(MainFunctionName, BindingFlags.Public
                                                                  | BindingFlags.InvokeMethod
                                                                  | BindingFlags.Instance | BindingFlags.Static, null,
                                                target,
                                                new object[] {t, y, tps});
            return res as Vector;
        }

        public double FunctionSet(double t, Vector y, int index, TaskParameters tp)
        {
            object res = classType.InvokeMember(SecondFunctionName, BindingFlags.Public
                                                                    | BindingFlags.InvokeMethod
                                                                    | BindingFlags.Instance | BindingFlags.Static, null,
                                                target,
                                                new object[] {t, y, tp[index]});
            return (double) res;
        }
    }
}