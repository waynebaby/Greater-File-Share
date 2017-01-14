using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreaterFileShare
{
    internal class Consts
    {
        public const string FilesRelativeUri = "Files";
        public const string ApiRelativeUri = "Api";
        public const int WCFPort = 8800;
        //netsh http add urlacl url=http://+:8800/ user=everyone
    }
}
