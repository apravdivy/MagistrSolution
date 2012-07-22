using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Task
{
    public sealed class TaskParameters : IEnumerable<TaskParameter>, ICloneable
    {
        private readonly List<TaskParameter> p;

        public TaskParameters(List<TaskParameter> list, object gamma)
        {
            p = list;
            Gamma = gamma;
        }

        public object Gamma { get; private set; }

        public TaskParameter this[int index]
        {
            get
            {
                if (index < 0 || index >= p.Count)
                {
                    throw new IndexOutOfRangeException("TaskParameters");
                }
                return p[index];
            }
        }

        public int Count
        {
            get { return p.Count; }
        }

        #region ICloneable Members

        public object Clone()
        {
            var ll = new List<TaskParameter>();
            foreach (TaskParameter tp in p)
            {
                var tpCopy = new TaskParameter(tp.Param.Length);
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

        #region IEnumerable<TaskParameter> Members

        public IEnumerator<TaskParameter> GetEnumerator()
        {
            return p.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return p.GetEnumerator();
        }

        #endregion

        public void Add(TaskParameter tp)
        {
            p.Add(tp);
        }
    }
}