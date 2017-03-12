using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreaterFileShare
{
    internal class Consts
    {
        public const string FilesRelativeUri = "files";
        public const string APIRelativeUri = "api";
        public const string ShortUrlRelativeUri = "s";
        public const string SwaggerRelativeUri = "swagger/ui"; 
        public const string WCFRelativeUri = "wcf";
        public const int WCFPort = 8800;
        public const string SettingExtension = ".gfssetting";
        //netsh http add urlacl url=http://+:8800/ user=everyone
    }
}
