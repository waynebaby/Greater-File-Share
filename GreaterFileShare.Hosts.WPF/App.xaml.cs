using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Threading;
using GreaterFileShare.Hosts.Core;
using GreaterFileShare.Hosts.WPF.Services;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Diagnostics;
using GreaterFileShare.Services;
using System.Collections.ObjectModel;
using GreaterFileShare.Hosts.WPF.Models;
using GreaterFileShare.WCF;
using GreaterFileShare.WCF.Models;
using System.Collections.Concurrent;
using MVVMSidekick.EventRouting;
using System.Windows.Navigation;
using Windows.Storage;

namespace GreaterFileShare.Hosts.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static string[] CommandLineArgs = Environment.GetCommandLineArgs();
        public App()
        {

            MVVMSidekick.EventRouting.EventRouter.Instance.GetEventChannel<(ShareFileTask ViewModel, bool IsHosted)>().Subscribe(
                    ep =>
                    {

                        (ShareFileTask viewModel, bool isHosted) = ep.EventData;
                        var urlRoot = new Uri($"http://localhost:{viewModel.Port}/{Consts.FilesRelativeUri}");
                        var hosts = ServiceLocator.Instance.Resolve<ConcurrentDictionary<string, HostItem>>();
                        if (isHosted)
                        {
                            EventRouter.Instance.RaiseEvent<string>(this, "Adding Host to WCF Service List", "Logging");
                            var item = new HostItem
                            {
                                LocalFilePath = viewModel.Path,
                                DirectorySeparatorChar = '/',
                                UrlRoot = urlRoot
                            };
                            var exists = !hosts.TryAdd(item.UrlRoot.ToString(), item);

                            EventRouter.Instance.RaiseEvent<string>(this, exists ? "Item Exists" : "Add Successed", "Logging");
                        }
                        else
                        {
                            EventRouter.Instance.RaiseEvent<string>(this, "Removing Host to WCF Service List", "Logging");

                            var exists = hosts.TryRemove(urlRoot.ToString(), out var x);
                            EventRouter.Instance.RaiseEvent<string>(this, exists ? "Remove Successed" : "Not Found", "Logging");
                        }


                    });


        }

        public static void InitNavigationConfigurationInThisAssembly()
        {

            MVVMSidekick.Startups.StartupFunctions.RunAllConfig();

            ServiceLocator.Instance.Register<ILauncher, Launcher>();
            ServiceLocator.Instance.Register<ISettingRepoService<ObservableCollection<ShareFileTask>>, SettingRepoService<ObservableCollection<ShareFileTask>>>();
            ServiceLocator.Instance.Register<ISettingRepoService<ObservableCollection<ShareFileTask>>, Win32SettingRepoService<ObservableCollection<ShareFileTask>>>("W32");
            ServiceLocator.Instance.Register<INetworkService, NetworkService>();
            ServiceLocator.Instance.Register<IFileSystemHubService, FileSystemHubService>();
            var fileService = new SharingFileCatalogWebSiteService();
            ServiceLocator.Instance.Register<ISharingFileCatalogService>(fileService);
            ServiceLocator.Instance.Register(fileService.Hosts);
            ServiceLocator.Instance.Register(nameof(CommandLineArgs), CommandLineArgs);
            ServiceLocator.Instance.Register<IStorageItem, EmptyIStorageItem>(nameof(SettingRepoService<ShareFileTask>.SuggestedTargetStorageItem));
            ServiceLocator.Instance.Register<string>(nameof(Win32SettingRepoService<ShareFileTask>.SuggestedTargetFilePath), string.Empty);

            ServiceLocator.Instance.RegisterFactory<IFileSystemService>(null,
                (o, s) =>
                {
                    return new FileSystemService(o.ToString());
                }
                );

        }

        /// <summary>
        /// WCF Host
        /// </summary>
        ServiceHost host;
        protected override void OnStartup(StartupEventArgs e)
        {

            InitNavigationConfigurationInThisAssembly();
            host = GetServiceHost();
            Debug.WriteLine(host.BaseAddresses.FirstOrDefault()?.ToString());
            try
            {
                host.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error On Startup:");
                Environment.Exit(0);
            }



            base.OnStartup(e);


        }


        protected override void OnExit(ExitEventArgs e)
        {
            var mainVM = Resources["DesignVM"] as ViewModels.MainWindow_Model;
            var hosts = mainVM?.HostingTasks?
                .Select(x =>
                {

                    try
                    {
                        x.Stop();
                        return true;

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                })?.ToList();


            host?.Close();
            base.OnExit(e);

        }





        /// <summary>
        /// 生成Host
        /// </summary>
        /// <param name="item"></param>
        /// <param name="iclass"></param>
        /// <returns></returns>
        private ServiceHost GetServiceHost()
        {
            Uri uriAddress = null;
            ServiceHost host = null;
            var newClass = ServiceLocator.Instance.Resolve<ISharingFileCatalogService>();


            host = new ServiceHost(newClass);//实例一个服务，初始化终结点的服务对象和主机地址
            //host.AddServiceEndpoint(typeof(IFileSystemHubService), binding, nameof(FileSystemHubService));//实例化一个终结点，初始化服务接口，协议类型，地址
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

            //smb.HttpGetEnabled = false;
            //host.Description.Behaviors.Add(smb);
            //host.AddServiceEndpoint(typeof(IMetadataExchange), GetTcpMexBinding(), "mex");



            return host;
        }



    }
}
