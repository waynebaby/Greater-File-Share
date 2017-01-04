using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var p = Process.GetProcessesByName("explorer").FirstOrDefault();
            Console.WriteLine("Found process:{0}", p != null);
            var filename = p.MainModule.FileName ;
            p.Kill();

            //p.WaitForExit();

            Process.Start(new ProcessStartInfo { FileName = filename });



            Console.Read();
        }
    }
}
