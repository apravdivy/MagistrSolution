using System;
using System.Windows.Forms;

namespace View.Forms
{
    public partial class EnterNameForm : Form
    {
        public string name;

        public EnterNameForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}