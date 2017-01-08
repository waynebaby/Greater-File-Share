using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.Core
{
    public interface ILauncher
    {
        Task RunWebsiteAsync(string contentFolderPath, int port = 8080, IDictionary<string, string> additionalConentTypes = null, CancellationToken cancellationToken = default(CancellationToken));
       }
}