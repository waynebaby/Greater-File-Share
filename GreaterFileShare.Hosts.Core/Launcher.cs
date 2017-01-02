using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.Core
{
    public class Launcher
    {

        public async Task RunWebsiteAsync(string contentFolderPath, int port = 8080, CancellationToken cancellationToken = default(CancellationToken))
        {
            await GreaterFileShare.Web.Program.StartAsync(contentFolderPath, 5000, cancellationToken);
            //var workingFolder = Path.GetDirectoryName(this.GetType().Assembly.Location);
            //var startInfo = new ProcessStartInfo
            //{
            //    Arguments = contentFolderPath,
            //    FileName = Path.Combine(workingFolder, "GreaterFileShare.Web.exe"),
            //    WorkingDirectory = workingFolder,
            //};
            //var p = new Process()
            //{
            //    StartInfo = startInfo
            //};


            //p.Start();


            //var tcs = new TaskCompletionSource<object>();

            //p.Exited += (o, a) =>
            //{

            //    tcs.TrySetCanceled();
            //};

            //cancellationToken.Register(() =>
            //{
            //    p.Kill();
            //});

            //await tcs.Task;


        }
    }
}
