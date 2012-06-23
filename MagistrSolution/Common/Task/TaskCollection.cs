using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Common.Task
{
    [DataContract]
    public class TaskCollection : IEnumerable<TaskDescription>
    {
        [DataMember]
        private List<TaskDescription> tasks = new List<TaskDescription>();

        public IEnumerator<TaskDescription> GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        public void Add(TaskDescription t)
        {
            this.tasks.Add(t);
        }

        public void Remove(TaskDescription t)
        {
            this.tasks.Remove(t);
        }

        public void RemoveAt(int i)
        {
            this.tasks.RemoveAt(i);
        }

        public TaskDescription this[int i]
        {
            get 
            {
                return this.tasks[i];
            }
            set 
            {
                this.tasks[i] = value;
            }
        }


    }
}
