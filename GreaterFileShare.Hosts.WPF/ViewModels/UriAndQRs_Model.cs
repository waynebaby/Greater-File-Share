using System.Reactive;
using System.Reactive.Linq;
using MVVMSidekick.ViewModels;
using MVVMSidekick.Views;
using MVVMSidekick.Reactive;
using MVVMSidekick.Services;
using MVVMSidekick.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using GreaterFileShare.Hosts.WPF.Services;
using GreaterFileShare.Hosts.WPF.Models;

namespace GreaterFileShare.Hosts.WPF.ViewModels
{

    public class UriAndQRs_Model : ViewModelBase<UriAndQRs_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public UriAndQRs_Model()
        {

            this.ListenChanged(x => x.CurrentTask, x => x.SelectedHost)
               .ObserveOnDispatcher()
                 .Do(e =>
               {
                   Urls = new Models.UrlGroup(SelectedHost??"localhost", CurrentTask?.Port ?? 80);
               })
               .Subscribe()
               .DisposeWith(this);
            ;



        }



        public UrlGroup Urls
        {
            get { return _UrlsLocator(this).Value; }
            set { _UrlsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property UrlGroup Urls Setup        
        protected Property<UrlGroup> _Urls = new Property<UrlGroup> { LocatorFunc = _UrlsLocator };
        static Func<BindableBase, ValueContainer<UrlGroup>> _UrlsLocator = RegisterContainerLocator<UrlGroup>(nameof(Urls), model => model.Initialize(nameof(Urls), ref model._Urls, ref _UrlsLocator, _UrlsDefaultValueFactory));
        static Func<UrlGroup> _UrlsDefaultValueFactory = () => new UrlGroup();
        #endregion


        //propvm tab tab string tab Title


        public GreaterFileShare.Hosts.WPF.Models.ShareFileTask CurrentTask
        {
            get { return _CurrentTaskLocator(this).Value; }
            set { _CurrentTaskLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property GreaterFileShare.Hosts.WPF.Models.ShareFileTask CurrentTask Setup        
        protected Property<GreaterFileShare.Hosts.WPF.Models.ShareFileTask> _CurrentTask = new Property<GreaterFileShare.Hosts.WPF.Models.ShareFileTask> { LocatorFunc = _CurrentTaskLocator };
        static Func<BindableBase, ValueContainer<GreaterFileShare.Hosts.WPF.Models.ShareFileTask>> _CurrentTaskLocator = RegisterContainerLocator<GreaterFileShare.Hosts.WPF.Models.ShareFileTask>(nameof(CurrentTask), model => model.Initialize(nameof(CurrentTask), ref model._CurrentTask, ref _CurrentTaskLocator, _CurrentTaskDefaultValueFactory));
        static Func<GreaterFileShare.Hosts.WPF.Models.ShareFileTask> _CurrentTaskDefaultValueFactory = () => default(GreaterFileShare.Hosts.WPF.Models.ShareFileTask);
        #endregion


        public ObservableCollection<string> Hosts
        {
            get { return _HostsLocator(this).Value; }
            set { _HostsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<string> Hosts Setup        
        protected Property<ObservableCollection<string>> _Hosts = new Property<ObservableCollection<string>> { LocatorFunc = _HostsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<string>>> _HostsLocator = RegisterContainerLocator<ObservableCollection<string>>(nameof(Hosts), model => model.Initialize(nameof(Hosts), ref model._Hosts, ref _HostsLocator, _HostsDefaultValueFactory));
        static Func<ObservableCollection<string>> _HostsDefaultValueFactory = () => default(ObservableCollection<string>);
        #endregion



        public string SelectedHost
        {
            get { return _SelectedHostLocator(this).Value; }
            set { _SelectedHostLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string SelectedHost Setup        
        protected Property<string> _SelectedHost = new Property<string> { LocatorFunc = _SelectedHostLocator };
        static Func<BindableBase, ValueContainer<string>> _SelectedHostLocator = RegisterContainerLocator<string>(nameof(SelectedHost), model => model.Initialize(nameof(SelectedHost), ref model._SelectedHost, ref _SelectedHostLocator, _SelectedHostDefaultValueFactory));
        static Func<string> _SelectedHostDefaultValueFactory = () => default(string);
        #endregion

        protected override async Task OnBindedViewLoad(IView view)
        {
            var nh = ServiceLocator.Instance.Resolve<INetworkService>();
            if (CurrentTask == null)
            {
                throw new InvalidOperationException("need a CurrentTask instance first");
            }
            Hosts = new ObservableCollection<string>(nh.GetHosts());
            await base.OnBindedViewLoad(view);
        }





        public CommandModel<ReactiveCommand, String> CommandLaunch
        {
            get { return _CommandLaunchLocator(this).Value; }
            set { _CommandLaunchLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandLaunch Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandLaunch = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandLaunchLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandLaunchLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandLaunch), model => model.Initialize(nameof(CommandLaunch), ref model._CommandLaunch, ref _CommandLaunchLocator, _CommandLaunchDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandLaunchDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandLaunch);           // Command resource  
                var commandId = nameof(CommandLaunch);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            if (e.EventArgs.Parameter is string )
                            {
                                await Windows.System.Launcher.LaunchUriAsync(new Uri(e.EventArgs.Parameter as string));
                            }
                            //Todo: Add Launch logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
                return cmdmdl;
            };

        #endregion


    }

}

