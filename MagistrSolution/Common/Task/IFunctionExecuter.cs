using Common.Mathematic;

namespace Common
{
    public interface IFunctionExecuter
    {
        int SetCount { get; }
        Vector AbstractFunction(double t, Vector y);
        double FunctionSet(double t, Vector y, int index);
    }
}