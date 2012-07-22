using System;
using System.Threading;
using System.Windows.Forms;
using Core;

namespace SystemStarter
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            IView view = new View.View();
            var controler = new Controler(view);
            controler.Start();
            while (controler.Alive)
            {
                try
                {
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                catch
                {
                    MessageBox.Show("DoEvents errror");
                }
            }
        }
    }
}