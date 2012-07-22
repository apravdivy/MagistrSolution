using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FuncDescrAttribute : Attribute
    {
        public FuncDescrAttribute(int n)
        {
            this.N = n;
        }

        public int N { get; set; }

        public string[] Methods { get; set; }
    }
}