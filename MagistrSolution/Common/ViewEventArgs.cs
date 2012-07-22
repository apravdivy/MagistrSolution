using System.Collections.Generic;

namespace Common
{
    public class ViewEventArgs
    {
        private readonly List<object> parameters;

        public ViewEventArgs(object[] p)
        {
            parameters = new List<object>();
            parameters.AddRange(p);
        }

        public List<object> Parameters
        {
            get { return parameters; }
        }
    }
}