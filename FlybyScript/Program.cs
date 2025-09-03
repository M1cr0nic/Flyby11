using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flyby11
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            var headless = args.Any(a => a.Equals("--headless", StringComparison.OrdinalIgnoreCase));
            var isoArg = args.FirstOrDefault(a => a.StartsWith("--iso=", StringComparison.OrdinalIgnoreCase));
            var isoPath = isoArg?.Substring("--iso=".Length).Trim('"');

            if (headless)
            {
                return HeadlessUpgrade.Run(isoPath);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            return 0;
        }
    }
}
