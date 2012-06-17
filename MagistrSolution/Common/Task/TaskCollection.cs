using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class TaskCollection : IEnumerable<Task>
    {
        [DataMember]
        private List<Task> tasks = new List<Task>();

        public IEnumerator<Task> GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return tasks.GetEnumerator();
        }

        public void Add(Task t)
        {
            this.tasks.Add(t);
        }

        public void Remove(Task t)
        {
            this.tasks.Remove(t);
        }

        public void RemoveAt(int i)
        {
            this.tasks.RemoveAt(i);
        }

        public Task this[int i]
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
