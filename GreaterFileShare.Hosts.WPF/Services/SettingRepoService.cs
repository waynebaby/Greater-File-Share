using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public class SettingRepoService<T> : ISettingRepoService<T>
    {
        public string Name
        {
            get;set;
        }

        public Task<T> LoadAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(T entry)
        {
            throw new NotImplementedException();
        }
    }
}
