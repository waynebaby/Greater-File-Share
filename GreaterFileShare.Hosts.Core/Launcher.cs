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
    public class Launcher : ILauncher
    {

        public async Task RunWebsiteAsync(string contentFolderPath, int port = 8080, CancellationToken cancellationToken = default(CancellationToken))
        {
            await GreaterFileShare.Web.Program.StartAsync(contentFolderPath, port , cancellationToken);
        
        }
    }
}
