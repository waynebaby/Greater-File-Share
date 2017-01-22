using GreaterFileShare.Hosts.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.System;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public class NetworkService : INetworkService
    {
        public IList<HostEntry> GetHosts()
        {
            var hns = NetworkInformation.GetHostNames();
            var names = hns.Select(
                x =>
                {
                     return new HostEntry
                    {
                        HostName = x.CanonicalName,
                        AdapterName = x.Type.ToString()
                    };
                }

            ).ToList();
         


            return names;
        }

        public async Task LaunchUri(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
