using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.WCF.Models
{

    [DataContract]
    public class ItemBase
    {

        [DataMember] public string Name { get; set; }
        [DataMember] public string FullPath { get; set; }
        [DataMember] public string RelativePath { get; set; }

    }
}
