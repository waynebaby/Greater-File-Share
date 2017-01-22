using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Services
{
    [ServiceContract]
    public interface IFileSystemHubService
    {
        [OperationContract]
        Task<IList<GreaterFileShare.Services.FileEntry>> GetFilesAsync(int port, string pathFolder);


        [OperationContract]
        Task<IList<GreaterFileShare.Services.FolderEntry>> GetFoldersAsync(int port, string pathFolder);


        [OperationContract]
        Task<string> GetDefaultFolderAsync();
    }
}
