using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Flyby11
{
    public class HeadlessUpgrade
    {
        public Stream stdoutStream;
        public StreamWriter stdoutWriter;
        public HeadlessUpgrade()
        {
            // Get the standard output stream
            stdoutStream = Console.OpenStandardOutput();
            // Wrap it in a StreamWriter for text output
            stdoutWriter = new StreamWriter(stdoutStream);
            stdoutWriter.WriteLine("Hello, stdout!");
            Console.WriteLine("Hello, World!");
        }

        public async void Run(string isoPath) {
            var isoHandler = new IsoHandler(stdout);
            await isoHandler.HandleIso(isoPath, false);
        }

        public void stdout(string update)
        {
            stdoutWriter.WriteLine(update);
        }
    }
}
