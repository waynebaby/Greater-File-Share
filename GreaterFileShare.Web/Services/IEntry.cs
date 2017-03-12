using System.Runtime.Serialization;

namespace GreaterFileShare.Services
{
    public class Entry
    {
      [DataMember]  public  string FullPath { get; set; }
      [DataMember]  public  string Name { get; set; }
      [DataMember]  public  string RelativeUri { get; set; }
        [DataMember] public  string ShortUriKey { get; set; }
    }
}