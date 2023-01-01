using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PulsarRunner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Screen screen = Screen.FromPoint(Cursor.Position);
            Application.Run(new Form1() {
                StartPosition = FormStartPosition.CenterScreen, //Summary in VS actually mentions this as needed to make use of Location
                Location = screen.Bounds.Location,
        });
        }
    }
}
