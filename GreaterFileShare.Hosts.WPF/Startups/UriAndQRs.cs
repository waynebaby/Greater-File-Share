using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using GreaterFileShare.Hosts.WPF;
using GreaterFileShare.Hosts.WPF.ViewModels;
using System;
using System.Net;
using System.Windows;


namespace MVVMSidekick.Startups
{
    internal static partial class StartupFunctions
    {
        static Action UriAndQRsConfig =
            CreateAndAddToAllConfig(ConfigUriAndQRs);

        public static void ConfigUriAndQRs()
        {
            ViewModelLocator<UriAndQRs_Model>
                .Instance
                .Register(context =>
                    new UriAndQRs_Model())
                .GetViewMapper()
                .MapToDefault<UriAndQRs>();

        }
    }
}
