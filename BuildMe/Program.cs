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

        static void Main(string[] args)
        {


            Console.WriteLine("Changing AppManifest File Fields...");
            bool forTestingSign;
            forTestingSign = args.Any(x => x.Equals(nameof(forTestingSign), StringComparison.InvariantCultureIgnoreCase));
            ChangeAppManifestFileFields(args.FirstOrDefault(), forTestingSign);


        }


        const string AppManifestFile = "AppxManifest.xml";

        static void ChangeAppManifestFileFields(string folderPath, bool forTestingSign)
        {
            var filepath = Path.Combine(folderPath, AppManifestFile);  //打开manifest文件

            XDocument d = XDocument.Load(filepath);

            ConfigTestSignPublisherName(forTestingSign, d); //设置测试签名的发布人

            ConfigResources(d); //设置语言与资源

            ConfigVersion(d); //设置版本

            ConfigFirewall(d);   //设置防火墙和文件扩展


            d.Save(filepath);
        }



        private static void ConfigTestSignPublisherName(bool forTestSign, XDocument d)
        {
            if (forTestSign)
            {
                var ele = d.Descendants().FirstOrDefault(e => e.Name.LocalName == "Identity");
                ChangeAttribute(ele, "Publisher", "CN=Appx Test Root Agency Ex");
            }
        }

        static void ChangeAttribute(XElement target, string field, string value)  //设定xml赋值函数
        {
            target.Attributes().Where(x => x.Name.LocalName == field).First().Value = value;
            Console.WriteLine($"{field} Changed to \"{value}\"...");
        }

        private static void ConfigVersion(XDocument d)
        {
            var targetFamily = d.Descendants().First(x => x.Name.LocalName == "TargetDeviceFamily");
            ChangeAttribute(targetFamily, "MaxVersionTested", "10.0.16299.0");
            ChangeAttribute(targetFamily, "MinVersion", "10.0.15062.0");        //新版本才支持防火墙
        }

        private static void ConfigResources(XDocument d)
        {
            var ve = d.Descendants().Where(x => x.Name.LocalName == "VisualElements").First(); //找到tile定义

            ChangeAttribute(ve, "DisplayName", "ms-resource:Resources/AppName");                 //将定义修改为资源访问
            ChangeAttribute(ve, "Description", "ms-resource:Resources/AppDescription");          //将定义修改为资源访问


            var rse = d.Descendants().FirstOrDefault(x => x.Name.LocalName == "Resources");      //找到资源的定义
            rse.Add(new XElement(
                            XName.Get("Resource", rse.Name.NamespaceName),                      //添加支持的语言定义
                            new XAttribute(XName.Get("Language"), "zh-Hans")));
            rse.Add(new XElement(
                   XName.Get("Resource", rse.Name.NamespaceName),
                            new XAttribute(XName.Get("Language"), "zh-Hant")));
        }

        private static void ConfigFirewall(XDocument d)
        {
            var firewallNamespace = "http://schemas.microsoft.com/appx/manifest/desktop/windows10/2";
            var fileTypeAssociationNamespace = "http://schemas.microsoft.com/appx/manifest/uap/windows10/3";
            var executable = "bin\\GreaterFileShare.Hosts.WPF.exe";
            var proot = d.Root;
            var outsideAppExtensions = proot.Elements().FirstOrDefault(x =>
                 x.Name.LocalName == "Extensions");
            if (outsideAppExtensions == null)
            {
                proot.Add(
                    outsideAppExtensions = new XElement(XName.Get("Extensions", proot.Name.NamespaceName)));

            }


            var extension = outsideAppExtensions.Elements().FirstOrDefault(x =>
                x.Name.LocalName == "Extension"
                && x.Attributes().FirstOrDefault(a => a.Name.LocalName == "Category")?.Value == "windows.firewallRules"
                );
            if (extension == null)
            {
                extension = new XElement(XName.Get("Extension", firewallNamespace),
                    new XAttribute("Category", "windows.firewallRules"));
                outsideAppExtensions.Add(extension);
            }


            XElement firewall = null;
            if ((firewall = extension.Elements().FirstOrDefault(x => x.Name.LocalName == "FirewallRules")) == null)
            {
                firewall = new XElement(XName.Get("FirewallRules", firewallNamespace),
                    new XAttribute("Executable", executable));

                extension.Add(firewall);
            }

            XElement rule = null;
            if ((rule = firewall.Elements().FirstOrDefault(x => x.Name.LocalName == "Rule")) == null)
            {
                rule = XElement.Parse(@"
    <Rule
      Direction=""in""
      IPProtocol = ""TCP""
      Profile = ""all"" />
");
                rule.Name = XName.Get(rule.Name.LocalName, firewall.Name.NamespaceName);
                firewall.Add(rule);

                rule = XElement.Parse(@"
    <Rule
      Direction=""out""
      IPProtocol = ""TCP""
      Profile = ""all"" />
");
                rule.Name = XName.Get(rule.Name.LocalName, firewall.Name.NamespaceName);

                firewall.Add(rule);

            }


            //            var xml =@"
            //<uap:Extension Category=""windows.fileTypeAssociation"">
            //<uap3:FileType AssociationName=""Contoso"">
            //<uap:SupportedFileTypes>
            //<uap:FileType>.txt</uap:FileType>
            //<uap:FileType>.avi</uap:FileType>
            //</uap:SupportedFileTypes>
            //</uap3:FileTypeAssociation>
            //</uap:Extension>";


            var application = proot.Descendants().Single(x => x.Name.LocalName == "Application" && x?.Parent?.Parent == proot);
            var insideExtensions = application.Elements().FirstOrDefault(x => x.Name.LocalName == "Extensions");
            if (insideExtensions == null)
            {
                insideExtensions = new XElement(XName.Get("Extensions", proot.Name.Namespace.NamespaceName));
                application.Add(insideExtensions);
            }
            var extensionFile = insideExtensions.Elements().FirstOrDefault(x =>
                x.Name.LocalName == "Extension"
                && x.Attributes().FirstOrDefault(a => a.Name.LocalName == "Category")?.Value == "windows.fileTypeAssociation"

                );
            if (extensionFile == null)
            {
                extensionFile = new XElement(XName.Get("Extension", "http://schemas.microsoft.com/appx/manifest/uap/windows10/3"),
                    new XAttribute("Category", "windows.fileTypeAssociation"));
                insideExtensions.Add(extensionFile);
            }

            //gfssetting;

            XElement fileTypeAssociation = null;
            if ((fileTypeAssociation = extensionFile.Elements().FirstOrDefault(x => x.Name.LocalName == "fileTypeAssociation")) == null)
            {
                // var xml = @"<SupportedFileTypes>
                //  <FileType>.gfssetting</FileType>
                //</SupportedFileTypes>";

                fileTypeAssociation =
                    new XElement
                    (
                        XName.Get("FileTypeAssociation", fileTypeAssociationNamespace),
                        new XAttribute("Name", "settings"),
                        new XElement
                        (
                            XName.Get("SupportedFileTypes", "http://schemas.microsoft.com/appx/manifest/uap/windows10"),
                            new XElement
                            (
                                XName.Get("FileType", "http://schemas.microsoft.com/appx/manifest/uap/windows10"),
                                ".gfssetting"
                            )
                        )
                    );

                extensionFile.Add(fileTypeAssociation);
            }
        }
    }
}
