using System;
using DevExpress.XtraEditors;

namespace View
{
    public sealed class OutputHelper
    {
        private readonly MemoEdit memoEdit;

        public OutputHelper(MemoEdit me)
        {
            memoEdit = me;
        }

        public void WriteLine(string s)
        {
            if (memoEdit.InvokeRequired)
            {
                memoEdit.BeginInvoke(new Action<string>(WriteLine), s);
            }
            else
            {
                memoEdit.Text += s + "\r\n";
                memoEdit.Select(memoEdit.Text.Length, memoEdit.Text.Length);
                memoEdit.ScrollToCaret();
            }
        }

        public void ClearScreen()
        {
            if (memoEdit.InvokeRequired)
            {
                memoEdit.BeginInvoke(new Action(ClearScreen));
            }
            else
            {
                memoEdit.Text = string.Empty;
                memoEdit.Select(memoEdit.Text.Length, memoEdit.Text.Length);
                memoEdit.ScrollToCaret();
            }
        }
    }
}