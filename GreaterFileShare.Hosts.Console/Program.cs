using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(typeof(Program).Assembly.Location);



            var l = new Core.Launcher();

            //l.RunWebsiteAsync(
            //    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.GetDirectories("contents").FirstOrDefault()?.FullName,
            //    5000,
            //    default(CancellationToken)).Wait();

            System.Console.Read();
        }
    }
}
