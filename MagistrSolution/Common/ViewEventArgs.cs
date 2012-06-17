using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class ViewEventArgs
    {
        private List<object> parameters;
        public List<object> Parameters
        {
            get { return parameters; }
        }
        public ViewEventArgs(object[] p)
        {
            this.parameters = new List<object>();
            this.parameters.AddRange(p);

        }
    }
}
