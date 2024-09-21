using System;
using System.Windows.Forms;

namespace DLLInjector
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); // تأكد من تغيير Form1 إلى MainForm هنا
        }
    }
}
