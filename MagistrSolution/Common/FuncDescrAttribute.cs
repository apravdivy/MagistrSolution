using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FuncDescrAttribute : Attribute
    {
        private int n;
        public int N
        {
            get { return n; }
            set { n = value; }
        }

        public string[] Methods { get; set; }

        public FuncDescrAttribute(int n)
        {
            this.n = n;
        }
    }

}
