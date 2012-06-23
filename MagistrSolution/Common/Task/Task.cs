using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Mathematic;
using System.Runtime.Serialization;


namespace Common
{
    [DataContract]
    public sealed class TaskDescription
    {
        [DataMember]
        public string name;

        [DataMember]
        public double t0;

        [DataMember]
        public double t1;

        [DataMember]
        public Vector startPoint;

        [DataMember]
        public List<List<double>> parametrs;

        [DataMember]
        public List<double> gammaSet;

        [DataMember]
        public MainFuncSetCount mainFuncSetCount;

        [DataMember]
        public SecondFuncType secFuncType;

        public TaskDescription()
        {

        }


    }
}
