using System.Runtime.Serialization;

namespace GreaterFileShare.Services
{
    [DataContract]
    public class FolderEntry
    {
        [DataMember]

        public string FullPath { get; internal set; }
        [DataMember]
        public string Name { get; internal set; }
    }
}