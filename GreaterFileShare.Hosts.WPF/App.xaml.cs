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
        }

        protected override async void OnStartup(StartupEventArgs e)
        {

            InitNavigationConfigurationInThisAssembly();
            base.OnStartup(e);
            var path = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.GetDirectories("contents").FirstOrDefault()?.FullName;
            var l = new GreaterFileShare.Hosts.Core.Launcher();
            //var t = l.RunWebsiteAsync(
            //        path,
            //        5000,
            //        default(CancellationToken));
            var s = new GreaterFileShare.Hosts.WPF.Models.ShareFileTask() ;
            s.Start(path, 5000);

        }
    }
}
