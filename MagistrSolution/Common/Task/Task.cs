using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Mathematic;

namespace Common
{
    [DataContract]
    public sealed class TaskDescription
    {
        [DataMember] public List<double> gammaSet;

        [DataMember] public MainFuncSetCount mainFuncSetCount;
        [DataMember] public string name;
        [DataMember] public List<List<double>> parametrs;

        [DataMember] public SecondFuncType secFuncType;
        [DataMember] public Vector startPoint;
        [DataMember] public double t0;

        [DataMember] public double t1;
    }
}