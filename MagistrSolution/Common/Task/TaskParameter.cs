﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Task
{
    public sealed class TaskParameter
    {
        public double A
        {
            get { return Param[0]; }
            set { Param[0] = value; }
        }
        public double B
        {
            get { return Param[1]; }
            set { Param[1] = value; }
        }

        public double[] Param;

        public TaskParameter()
        {
            Param = new double[2];
        }
        public TaskParameter(int count)
        {
            Param = new double[count];
        }
    }
}
