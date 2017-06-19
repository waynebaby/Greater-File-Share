using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using GreaterFileShare.WCF.Models;
using System.Collections.Concurrent;
using System.IO;
using System.Collections.ObjectModel;

namespace GreaterFileShare.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class SharingFileCatalogWebSiteService : ISharingFileCatalogService
    {

        object lockObj = new object();

        public SharingFileCatalogWebSiteService()
        {
            if (Instance == null)
            {
                lock (lockObj)
                {
                    if (Instance == null)
                    {
                        Instance = this;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

            }
            else
            {
                throw new Exception();
            }



        }




        public static SharingFileCatalogWebSiteService Instance { get; private set; }

        public ConcurrentDictionary<string, HostItem> Hosts = new ConcurrentDictionary<string, HostItem>();


        public async Task<FolderItem> GetFolderDetail(string hostUrl, string relativeFolderPath)
        {





            if (Hosts.TryGetValue(hostUrl, out var host))
            {

                var ServiceAddress = OperationContext.Current.IncomingMessageHeaders.To;
                var UrlBuilder = new UriBuilder(host.UrlRoot)
                {
                    Host = ServiceAddress.Host
                };

                var folder = new System.IO.DirectoryInfo(System.IO.Path.Combine(host.LocalFilePath ?? ".", relativeFolderPath.Trim('\\', '/')));
                if (folder.Exists)
                {
                    var folders = folder.EnumerateDirectories()
                        .Select(x => new FolderItem
                        {
                            Name = x.Name,
                            FullPath = $"{UrlBuilder.ToString()}{relativeFolderPath}{host.DirectorySeparatorChar.ToString()}{x.Name }",
                            RelativePath = $"{relativeFolderPath}{host.DirectorySeparatorChar.ToString()}{x.Name }",

                        });

                    var files = folder.EnumerateFiles()
                        .Select(x => new FileItem
                        {
                            Name = x.Name,
                            FullPath = $"{UrlBuilder.ToString()}{relativeFolderPath}{host.DirectorySeparatorChar.ToString()}{x.Name }",
                            RelativePath = $"{relativeFolderPath}{host.DirectorySeparatorChar.ToString()}{x.Name }",
                        });
                    return await Task.FromResult(new FolderItem
                    {
                        FullPath = folder.FullName,
                        Name = folder.Name,
                        Files = new Collection<FileItem>(files.ToList()),
                        Folders = new Collection<FolderItem>(folders.ToList()),
                        RelativePath = relativeFolderPath
                    });
                }



            }
            return null;


        }

        public async Task<IList<HostItem>> GetHosts()
        {
            return await Task.FromResult(this.Hosts.Values.ToList());
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Text;
//using System.Threading.Tasks;
//using GreaterFileShare.WCF.Models;
//using System.Collections.Concurrent;
//using System.IO;
//using System.Collections.ObjectModel;

//namespace GreaterFileShare.WCF
//{
//    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.

//    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
//    public class SharingFileCatalogWebSiteService : ISharingFileCatalogService
//    {

//        object lockObj = new object();

//        public SharingFileCatalogWebSiteService()
//        {
//            if (Instance == null)
//            {
//                lock (lockObj)
//                {
//                    if (Instance == null)
//                    {
//                        Instance = this;
//                    }
//                    else
//                    {
//                        throw new Exception();
//                    }
//                }

//            }
//            else
//            {
//                throw new Exception();
//            }

//#if DEBUG
//            //DesignTimeData
//            var s = new HostItem()
//            {

//                UrlRoot = new Uri("http://aaa/b/ccc")

//            };
//            Hosts[s.UrlRoot.ToString()] = s;
//#endif

//        }




//        public static SharingFileCatalogWebSiteService Instance { get; private set; }

//        public ConcurrentDictionary<string, HostItem> Hosts = new ConcurrentDictionary<string, HostItem>();


//        public Task<FolderItem> GetFolderDetail(string hostUrl, string relativeFolderPath)
//        {


//            if (Hosts.TryGetValue(hostUrl, out var host))
//            {
//                var folder = new System.IO.DirectoryInfo(System.IO.Path.Combine(host.LocalFilePath, relativeFolderPath));
//                if (folder.Exists)
//                {
//                    var folders = folder.EnumerateDirectories()
//                        .Select(x => new FolderItem
//                        {
//                            Name = x.Name,
//                            FullPath = $"{host.UrlRoot.ToString()}{host.DirectorySeparatorChar.ToString()}{x.Name }",
//                            RelativePath = $"{relativeFolderPath}{host.DirectorySeparatorChar.ToString()}{x.Name }"
//                        });

//                    var files = folder.EnumerateFiles()
//                        .Select(x => new FileItem
//                        {
//                            Name = x.Name,
//                            FullPath = x.FullName,
//                            RelativePath = Path.Combine(relativeFolderPath, x.Name)
//                        });
//                    return Task.FromResult(new FolderItem
//                    {
//                        FullPath = folder.FullName,
//                        Name = folder.Name,
//                        Files = new Collection<FileItem>(files.ToList()),
//                        Folders = new Collection<FolderItem>(folders.ToList()),
//                        RelativePath = relativeFolderPath
//                    });
//                }



//            }
//            return null;


//        }

//        public async Task<IList<HostItem>> GetHosts()
//        {
//            return await Task.FromResult(this.Hosts.Values.ToList());
//        }
//    }
//}
