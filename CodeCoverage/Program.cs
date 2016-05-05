using System;
using System.Windows.Forms;

namespace CodeCoverage
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new CodeCoverageContext());
        }
    }
}
