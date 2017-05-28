using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.WCF.Models
{

    [DataContract]
    public class HostItem
    {


        [DataMember]
        public string LocalFilePath
        {
            get; set;
        }


        [DataMember]
        public Uri UrlRoot
        {
            get; set;
        }

        [DataMember]

        public char DirectorySeparatorChar
        {
            get; set;
        }
    }
}
