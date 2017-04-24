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
            ChangeAppManifestFileFields(args.FirstOrDefault());


        }


        const string AppManifestFile = "AppxManifest.xml";

        static void ChangeAppManifestFileFields(string folderPath)
        {
            void ChangeAttribute(XElement target, string field, string value)  //设定xml赋值函数
            {
                target.Attributes().Where(x => x.Name.LocalName == field).First().Value = value;
                Console.WriteLine($"{field} Changed to \"{value}\"...");
            }

            var filepath = Path.Combine(folderPath, AppManifestFile);  //打开manifest文件

            XDocument d = XDocument.Load(filepath);
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

            //var targetFamily = d.Descendants().First(x=>x.Name.LocalName== "TargetDeviceFamily");
            //ChangeAttribute(targetFamily, "MaxVersionTested", "10.0.15063.0");         
            //ChangeAttribute(targetFamily, "MinVersion", "10.0.15063.0");        //新版本才支持防火墙


            //ConfigFirewall(d);   //设置防火墙

            d.Save(filepath);
        }

        private static void ConfigFirewall(XDocument d)
        {
            var firewallNamespace = "http://schemas.microsoft.com/appx/manifest/desktop/windows10/2";
            var executable = "bin\\GreaterFileShare.Hosts.WPF.exe";
            var proot = d.Root;
            var extenston = proot.Elements().FirstOrDefault(x =>
                x.Name.LocalName == "Extension"
                &&
                x.Name.NamespaceName == proot.Name.NamespaceName
                && x.Attributes().FirstOrDefault(a => a.Name.LocalName == "Category")?.Value == "windows.firewallRules"
                );
            if (extenston == null)
            {
                extenston = new XElement(XName.Get("Extension", firewallNamespace),
                    new XAttribute("Category", "windows.firewallRules"));
                proot.Add(extenston);
            }


            XElement firewall = null;
            if ((firewall = extenston.Elements().FirstOrDefault(x => x.Name.LocalName == "FirewallRules")) == null)
            {
                firewall = new XElement(XName.Get("FirewallRules", firewallNamespace),
                    new XAttribute("Executable", executable));

                extenston.Add(firewall);
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
        }
    }
}
