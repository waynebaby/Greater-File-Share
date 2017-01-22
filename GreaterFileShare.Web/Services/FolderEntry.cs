using System.Runtime.Serialization;

namespace GreaterFileShare.Services
{
    [DataContract]
    public class FolderEntry
    {
        [DataMember]

        public string FullPath { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}