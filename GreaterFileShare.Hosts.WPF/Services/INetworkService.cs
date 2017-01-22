using GreaterFileShare.Hosts.WPF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public interface INetworkService
    {
        IList<HostEntry> GetHosts();
        Task LaunchUri(Uri uri);
    }
}