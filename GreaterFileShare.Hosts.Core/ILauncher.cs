using System.Threading;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.Core
{
    public interface ILauncher
    {
        Task RunWebsiteAsync(string contentFolderPath, int port = 8080, CancellationToken cancellationToken = default(CancellationToken));
    }
}