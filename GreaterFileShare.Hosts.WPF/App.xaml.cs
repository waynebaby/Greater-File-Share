﻿using System.Reactive;
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

namespace GreaterFileShare.Hosts.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        public static void InitNavigationConfigurationInThisAssembly()
        {
            MVVMSidekick.Startups.StartupFunctions.RunAllConfig();

            ServiceLocator.Instance.Register<ILauncher, Launcher>();
            ServiceLocator.Instance.Register<ISettingRepoService<ObservableCollection<ShareFileTask>>, SettingRepoService<ObservableCollection<ShareFileTask>>>();
            ServiceLocator.Instance.Register<INetworkService, NetworkService>();
            ServiceLocator.Instance.Register<IFileSystemHubService, FileSystemHubService>();
            ServiceLocator.Instance.RegisterFactory<IFileSystemService>(null,
                (o, s) =>
                {
                    return new FileSystemService(o.ToString());
                }
                );

        }


        ServiceHost host;
        protected override async void OnStartup(StartupEventArgs e)
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
                MessageBox.Show(ex.Message,"Error On Startup:");
                Environment.Exit(0);
            }


            //net.tcp://+:8800/GreaterFileShare/Hosts/WPF/Services/FileSystemHubService

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
        /// 根据类型生成uri
        /// </summary>
        /// <param name="protocolType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private Uri GetUri(string protocolType, Type type)
        {
            Uri uri = null;
            switch (protocolType)
            {
                case "NetTcpBinding":
                    uri = new Uri($"net.tcp://localhost:{Consts.WCFPort}/{Consts.WCFRelativeUri}");
                    break;
            }
            return uri;
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
            var newClass = ServiceLocator.Instance.Resolve<IFileSystemHubService>();

            uriAddress = GetUri("NetTcpBinding", newClass.GetType());
            var binding = GetNetTcpBinding();
            host = new ServiceHost(newClass, uriAddress);//实例一个服务，初始化终结点的服务对象和主机地址
            host.AddServiceEndpoint(typeof(IFileSystemHubService), binding, nameof(FileSystemHubService));//实例化一个终结点，初始化服务接口，协议类型，地址
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

            smb.HttpGetEnabled = false;
            host.Description.Behaviors.Add(smb);
            host.AddServiceEndpoint(typeof(IMetadataExchange), GetTcpMexBinding(), "mex");



            return host;
        }

        #region 协议设置
        private NetTcpBinding GetNetTcpBinding()
        {
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.MaxBufferSize = 2147483647;
            netTcpBinding.MaxReceivedMessageSize = 2147483647;
            netTcpBinding.Security.Mode = SecurityMode.None;
            return netTcpBinding;
        }
        private Binding GetTcpMexBinding()
        {
            var mexBinding = MetadataExchangeBindings.CreateMexTcpBinding();
            return mexBinding;
        }

        private WSDualHttpBinding GetWSDualHttpBinding()
        {
            WSDualHttpBinding wsDualHttpBinding = new WSDualHttpBinding();
            wsDualHttpBinding.MaxReceivedMessageSize = 2147483647;
            return wsDualHttpBinding;
        }

        private BasicHttpBinding GetBasicHttpBingding()
        {
            BasicHttpBinding baseBinding = new BasicHttpBinding();
            baseBinding.MaxBufferSize = 2147483647;
            baseBinding.MaxReceivedMessageSize = 2147483647;
            return baseBinding;
        }
        #endregion
    }
}
