using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
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

    public sealed class TaskParameters : IEnumerable<TaskParameter>, ICloneable
    {
        public object Gamma { get; private set; }
        private List<TaskParameter> p = null;
        public TaskParameters(List<TaskParameter> list, object gamma)
        {
            this.p = list;
            this.Gamma = gamma;
        }

        public void Add(TaskParameter tp)
        {
            this.p.Add(tp);
        }

        public TaskParameter this[int index]
        {
            get
            {
                if (index < 0 || index >= this.p.Count)
                {
                    throw new IndexOutOfRangeException("TaskParameters");
                }
                return this.p[index];
            }
        }

        public int Count { get { return this.p.Count; } }

        #region IEnumerable<TaskParameter> Members

        public IEnumerator<TaskParameter> GetEnumerator()
        {
            return this.p.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.p.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            List<TaskParameter> ll = new List<TaskParameter>();
            foreach (TaskParameter tp in this.p)
            {
                TaskParameter tpCopy = new TaskParameter(tp.Param.Length);
                for (int i = 0; i < tp.Param.Length; i++)
                {
                    tpCopy.Param[i] = tp.Param[i];
                }
                ll.Add(tpCopy);
            }
            //this.p.ForEach(x => ll.Add(new TaskParameter() { A = x.A, B = x.B }));
            return new TaskParameters(ll, null);
        }

        #endregion
    }
}
