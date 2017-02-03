using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildMe
{
   public  class Program
    {
        static void Main(string[] args)
        {
            ChangeName(args.FirstOrDefault());
        }

        static void ChangeName(string filePath)
        {
            XDocument d =XDocument.Load(filePath);
            var ve = d.Descendants().Where(x => x.Name.LocalName == "VisualElements").First();
            ve.Attributes().Where(x => x.Name.LocalName == "DisplayName").First().Value = "Greater File Share";
            ve.Attributes().Where(x => x.Name.LocalName == "Description").First().Value = "File Sharing Server With QR support";

            
            d.Save(filePath);
        }
        
    }
}
