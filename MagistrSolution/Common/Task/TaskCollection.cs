using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Task
{
    [DataContract]
    public class TaskCollection : IEnumerable<TaskDescription>
    {
        [DataMember] private List<TaskDescription> tasks = new List<TaskDescription>();

        public TaskDescription this[int i]
        {
            get { return tasks[i]; }
            set { tasks[i] = value; }
        }

        #region IEnumerable<TaskDescription> Members

        public IEnumerator<TaskDescription> GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        #endregion

        public void Add(TaskDescription t)
        {
            tasks.Add(t);
        }

        public void Remove(TaskDescription t)
        {
            tasks.Remove(t);
        }

        public void RemoveAt(int i)
        {
            tasks.RemoveAt(i);
        }
    }
}