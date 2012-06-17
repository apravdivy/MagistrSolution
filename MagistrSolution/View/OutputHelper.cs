using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DevExpress.XtraEditors;

namespace View
{
    public sealed class OutputHelper
    {
        private delegate void EmptyDelegate();
        private delegate void StringDelegate(string s);
        private delegate void FormatDelegate(string format,object [] p);

        private MemoEdit memoEdit;

        public OutputHelper(MemoEdit me)
        {
            this.memoEdit = me;
        }

        public void WriteLine(string s)
        {
            if (this.memoEdit.InvokeRequired)
            {
                this.memoEdit.BeginInvoke(new StringDelegate(this.WriteLine),s);
            }
            else
            {
                this.memoEdit.Text +=s+"\r\n";
                this.memoEdit.Select(this.memoEdit.Text.Length, this.memoEdit.Text.Length);
                this.memoEdit.ScrollToCaret();
            }
        }
        //public void WriteLine(string f,object [] p)
        //{
        //    if (this.memoEdit.InvokeRequired)
        //    {
        //        this.memoEdit.BeginInvoke(new FormatDelegate(this.WriteLine), f,p);
        //    }
        //    else
        //    {
        //        this.memoEdit.Text +="\r\n";
        //    }
        //}
        public void ClearScreen()
        {
            if (this.memoEdit.InvokeRequired)
            {
                this.memoEdit.BeginInvoke(new EmptyDelegate(this.ClearScreen));
            }
            else
            {
                this.memoEdit.Text = string.Empty;
                this.memoEdit.Select(this.memoEdit.Text.Length, this.memoEdit.Text.Length);
                this.memoEdit.ScrollToCaret();
            }
        }
    }
}
