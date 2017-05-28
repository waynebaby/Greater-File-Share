using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.WCF.Models
{
    [DataContract]
    public class FolderItem : ItemBase
    {

        [DataMember]
        public ICollection<FileItem> Files
        {
            get; set;
        }



        [DataMember]
        public System.Collections.Generic.ICollection<FolderItem> Folders
        {
            get; set;
        }
    }
}
