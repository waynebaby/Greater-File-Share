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

namespace GreaterFileShare.Hosts.WPF.ViewModels
{

    public class MainWindow_Model : ViewModelBase<MainWindow_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        IDisposable _currentTaskListening;
        public MainWindow_Model()
        {

            this.GetValueContainer(x => x.CurrentTask)
                .GetEventObservable()
                .Subscribe(
                e =>
                {
                    var oldOne = e.EventArgs.OldValue;
                    _currentTaskListening?.Dispose();
                    var newOne = e.EventArgs.NewValue;
                    var hostingListen = newOne
                        .ListenChanged(x => x.IsHosting)
                        .ObserveOnDispatcher()
                        .Subscribe(w => IsUIBusy = CurrentTask.IsHosting);

                    _currentTaskListening = hostingListen;
                })
                .DisposeWith(this);

            CurrentTask.DisposeWith(this);

            var st = new ShareFileTask();
            HostingTasks.Add(st);
            CurrentTask = st;
        }



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



        public ObservableCollection<ContentTypePair> AdditionalContentTypes
        {
            get { return _AdditionalContentTypesLocator(this).Value; }
            set { _AdditionalContentTypesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ContentTypePair> AdditionalContentTypes Setup        
        protected Property<ObservableCollection<ContentTypePair>> _AdditionalContentTypes = new Property<ObservableCollection<ContentTypePair>> { LocatorFunc = _AdditionalContentTypesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ContentTypePair>>> _AdditionalContentTypesLocator = RegisterContainerLocator<ObservableCollection<ContentTypePair>>(nameof(AdditionalContentTypes), model => model.Initialize(nameof(AdditionalContentTypes), ref model._AdditionalContentTypes, ref _AdditionalContentTypesLocator, _AdditionalContentTypesDefaultValueFactory));
        static Func<ObservableCollection<ContentTypePair>> _AdditionalContentTypesDefaultValueFactory = () => new ObservableCollection<ContentTypePair>();
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



    



        public CommandModel<ReactiveCommand, String> CommandSelectPath
        {
            get { return _CommandSelectPathLocator(this).Value; }
            set { _CommandSelectPathLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSelectPath Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSelectPath = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSelectPathLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSelectPathLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSelectPath), model => model.Initialize(nameof(CommandSelectPath), ref model._CommandSelectPath, ref _CommandSelectPathLocator, _CommandSelectPathDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSelectPathDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandSelectPath);           // Command resource  
                var commandId = nameof(CommandSelectPath);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add SelectPath logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);


                return cmdmdl;
            };

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




    }
}

