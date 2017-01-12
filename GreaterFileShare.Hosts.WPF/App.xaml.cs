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
            ServiceLocator.Instance.Register<INewHostService, NewHostService>();
            ServiceLocator.Instance.Register<INetworkService, NetworkService>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {

            InitNavigationConfigurationInThisAssembly();
            base.OnStartup(e);
           

        }
    }
}
