using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Mathematic
{
    public delegate Vector FunctionDelegate(double x, Vector y);

    public delegate double FunctionForItegralDelegate(object obj);

    public delegate Vector RKMetodDelegate(double x, Vector y, double h);
    public delegate void RKResultGeneratedDelegate(RKResult res, string curveName);
    public delegate void RKSolvingDoneDelegate(RKResults res,string curveName,IFunctionExecuter fe);
}
