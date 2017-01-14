using System.Runtime.Serialization;

namespace GreaterFileShare.Services
{

    [DataContract]
    public class FileEntry
    {
        [DataMember]
        public string FullPath { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RelativeUri { get; set; }
    }
}