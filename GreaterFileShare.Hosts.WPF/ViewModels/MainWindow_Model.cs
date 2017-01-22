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
using GreaterFileShare.Hosts.WPF.Models;
using GreaterFileShare.Hosts.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using GreaterFileShare.Hosts.WPF.Services;

namespace GreaterFileShare.Hosts.WPF.ViewModels
{

    public class MainWindow_Model : ViewModelBase<MainWindow_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        //IDisposable _currentTaskListening;
        public MainWindow_Model()
        {

            if (IsInDesignMode)
            {

                Messages.Add(new Models.MessageEntry { Time = DateTime.Now, Message = "Message Here" });
                return;
            }
            GreaterFileShare.Hosts.WPF.Services.FileSystemHubService.vmInstance = this;
            var source1 = GlobalEventRouter.GetEventChannel<Exception>()
                    .Select(x => x.EventData.Message);

            var source2 = GlobalEventRouter.GetEventChannel<string>()
                    .Where(x => x.EventName == "Logging")
                    .Select(x => x.EventData);
            var source3 = new WebLoggingSource();

            new[] { source1, source2, source3 }
                .ToObservable()
                .SelectMany(ms => ms)
                .Select(x => new MessageEntry { Time = DateTime.Now, Message = x })
                .ObserveOnDispatcher()
                .Subscribe(x =>
                    {
                        Messages.Add(x);
                        if (Messages.Count > 500)
                        {
                            Messages.RemoveAt(0);
                        }

                        CurrentMessageIndex = Messages.Count - 1;
                    })
                .DisposeWith(this);
            CurrentTask.DisposeWith(this);
            var st = new ShareFileTask();
            HostingTasks.Add(st);
            CurrentTask = st;
          
        }



        public ObservableCollection<MessageEntry> Messages
        {
            get { return _MessagesLocator(this).Value; }
            set { _MessagesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<MessageEntry> Messages Setup        
        protected Property<ObservableCollection<MessageEntry>> _Messages = new Property<ObservableCollection<MessageEntry>> { LocatorFunc = _MessagesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<MessageEntry>>> _MessagesLocator = RegisterContainerLocator<ObservableCollection<MessageEntry>>(nameof(Messages), model => model.Initialize(nameof(Messages), ref model._Messages, ref _MessagesLocator, _MessagesDefaultValueFactory));
        static Func<ObservableCollection<MessageEntry>> _MessagesDefaultValueFactory = () => new ObservableCollection<MessageEntry>();
        #endregion



        public int CurrentMessageIndex
        {
            get { return _CurrentMessageIndexLocator(this).Value; }
            set { _CurrentMessageIndexLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int CurrentMessageIndex Setup        
        protected Property<int> _CurrentMessageIndex = new Property<int> { LocatorFunc = _CurrentMessageIndexLocator };
        static Func<BindableBase, ValueContainer<int>> _CurrentMessageIndexLocator = RegisterContainerLocator<int>(nameof(CurrentMessageIndex), model => model.Initialize(nameof(CurrentMessageIndex), ref model._CurrentMessageIndex, ref _CurrentMessageIndexLocator, _CurrentMessageIndexDefaultValueFactory));
        static Func<int> _CurrentMessageIndexDefaultValueFactory = () => default(int);
        #endregion



        public ShareFileTask CurrentTask
        {
            get { return _CurrentTaskLocator(this).Value; }
            set { _CurrentTaskLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ShareFileTask CurrentTask Setup        
        protected Property<ShareFileTask> _CurrentTask = new Property<ShareFileTask> { LocatorFunc = _CurrentTaskLocator };
        static Func<BindableBase, ValueContainer<ShareFileTask>> _CurrentTaskLocator = RegisterContainerLocator<ShareFileTask>(nameof(CurrentTask), model => model.Initialize(nameof(CurrentTask), ref model._CurrentTask, ref _CurrentTaskLocator, _CurrentTaskDefaultValueFactory));
        static Func<ShareFileTask> _CurrentTaskDefaultValueFactory = () => new ShareFileTask();
        #endregion





        public ObservableCollection<ShareFileTask> HostingTasks
        {
            get { return _HostingTasksLocator(this).Value; }
            set { _HostingTasksLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ShareFileTask> HostingTasks Setup        
        protected Property<ObservableCollection<ShareFileTask>> _HostingTasks = new Property<ObservableCollection<ShareFileTask>> { LocatorFunc = _HostingTasksLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ShareFileTask>>> _HostingTasksLocator = RegisterContainerLocator<ObservableCollection<ShareFileTask>>(nameof(HostingTasks), model => model.Initialize(nameof(HostingTasks), ref model._HostingTasks, ref _HostingTasksLocator, _HostingTasksDefaultValueFactory));
        static Func<ObservableCollection<ShareFileTask>> _HostingTasksDefaultValueFactory = () => new ObservableCollection<ShareFileTask>();
        #endregion





        public CommandModel<ReactiveCommand, String> CommandNewHost
        {
            get { return _CommandNewHostLocator(this).Value; }
            set { _CommandNewHostLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandNewHost Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandNewHost = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandNewHostLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandNewHostLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>("CommandNewHost", model => model.Initialize("CommandNewHost", ref model._CommandNewHost, ref _CommandNewHostLocator, _CommandNewHostDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandNewHostDefaultValueFactory =
            model =>
            {
                var resource = "CommandNewHost";           // Command resource  
                var commandId = "CommandNewHost";
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var t = new ShareFileTask();
                            if (vm.HostingTasks.Count > 0)
                            {
                                var hs = new HashSet<int>(vm.HostingTasks.Select(x => x.Port).Where(x => x.HasValue).Select(x => x.Value));
                                t.Port = vm.HostingTasks.Max(x => x.Port ?? 0) + 1;
                                while (hs.Contains(t.Port.Value))
                                {
                                    t.Port++;
                                    if (t.Port > 65535)
                                    {
                                        t.Port = 0;
                                    }
                                }
                            }
                            else
                            {
                                t.Port = 8080;
                            }
                            vm.HostingTasks.Add(t);
                            vm.CurrentTask = t;

                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);


                return cmdmdl;
            };

        #endregion



        public CommandModel<ReactiveCommand, String> CommandShowQR
        {
            get { return _CommandShowQRLocator(this).Value; }
            set { _CommandShowQRLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandShowQR Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandShowQR = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandShowQRLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandShowQRLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandShowQR), model => model.Initialize(nameof(CommandShowQR), ref model._CommandShowQR, ref _CommandShowQRLocator, _CommandShowQRDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandShowQRDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandShowQR);           // Command resource  
                var commandId = nameof(CommandShowQR);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var v2 = ServiceLocator.Instance.Resolve<UriAndQRs_Model>();
                            v2.CurrentTask = vm.CurrentTask;                           
                            await vm.StageManager.DefaultStage.Show(v2);
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                return cmdmdl;
            };

        #endregion



        protected override async Task OnBindedViewLoad(IView view)
        {

            var f = ServiceLocator.Instance.Resolve<IFileSystemHubService>();
            CurrentTask.Path = await f.GetDefaultFolderAsync();
            var cmd2 = CurrentTask.CommandStartHosting;
            var t= cmd2.ExecuteAsync(null);

            await base.OnBindedViewLoad(view);
        }
    }
}

