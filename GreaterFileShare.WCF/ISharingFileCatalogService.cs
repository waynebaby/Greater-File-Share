using GreaterFileShare.WCF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISharingFileCatalogService
    {
        [OperationContract]
        Task<IList<HostItem>> GetHosts();




        [OperationContract]
        Task<FolderItem> GetFolderDetail(string hostUrl, string relativeFolderPath);
       

    }
}
