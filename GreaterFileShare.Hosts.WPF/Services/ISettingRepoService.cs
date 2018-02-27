using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public interface ISettingRepoService<T>
    {

        string Name { get; set; }
        Task<T> LoadAsync();
        Task SaveAsync(T entry);
    }
}
