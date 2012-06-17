using System;
using Common.Mathematic;

namespace Common
{
    public interface IFunctionExecuter
    {
        Vector AbstractFunction(double t, Vector y);
        double FunctionSet(double t, Vector y, int index);
        int SetCount { get; }
    }
}
