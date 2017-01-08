using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public interface INetworkService
    {
        IList<string> GetHosts();
        Task LaunchUri(Uri uri);
    }
}