using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildMe
{
    public class Program
    {
        const string AppManifestFile = "AppxManifest.xml";

        static void Main(string[] args)
        {


            Console.WriteLine("Changing AppManifest File Fields...");
            ChangeAppManifestFileFields(args.FirstOrDefault());


        }

        static void ChangeAppManifestFileFields(string folderPath)
        {
            void ChangeAttribute(XElement target, string field, string value)
            {
                target.Attributes().Where(x => x.Name.LocalName == field).First().Value = value;
                Console.WriteLine($"{field} Changed to \"{ value}\"...");
            }
            var filepath = Path.Combine(folderPath, AppManifestFile);

            XDocument d = XDocument.Load(filepath);
            var ve = d.Descendants().Where(x => x.Name.LocalName == "VisualElements").First();

            ChangeAttribute(ve, "DisplayName", "ms-resource:Resources/AppName");
            ChangeAttribute(ve, "Description", "ms-resource:Resources/AppDescription");


            var rse = d.Descendants().FirstOrDefault(x => x.Name.LocalName == "Resources");
            rse.Add(new XElement(
                            XName.Get("Resource", rse.Name.NamespaceName),
                            new XAttribute(XName.Get("Language"), "zh-Hans")));
            rse.Add(new XElement(
                   XName.Get("Resource", rse.Name.NamespaceName),
                            new XAttribute(XName.Get("Language"), "zh-Hant")));

            d.Save(filepath);
        }


    }
}
