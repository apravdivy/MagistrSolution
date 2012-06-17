using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Core;
using View;

namespace SystemStarter
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            IView view = new View.View();
            Controler controler = new Controler(view);
            controler.Start();
            while (controler.Alive)
            {
                try
                {
                    Thread.Sleep(10);
                    System.Windows.Forms.Application.DoEvents();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("DoEvents errror");
                }
            }
        }
    }
}
