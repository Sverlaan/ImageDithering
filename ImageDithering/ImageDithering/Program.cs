using System;
using System.Windows.Forms;

namespace ImageDithering
{
    static class Program
    {
        // Run main program
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DitheringForm());
        }
    }
}
