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

        public MainWindow_Model()
        {

            this.CurrentTask.ListenChanged(x => x.IsHosting)
                .ObserveOnDispatcher()
                .Subscribe(w => IsUIBusy = CurrentTask.IsHosting)
                .DisposeWith(this);

            CurrentTask.DisposeWith(this);
        }



        public ShareFileTask CurrentTask
        {
            get { return _CurrentTaskLocator(this).Value; }
            private set { _CurrentTaskLocator(this).SetValueAndTryNotify(value); }
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



        public CommandModel<ReactiveCommand, String> CommandStartHosting
        {
            get { return _CommandStartHostingLocator(this).Value; }
            set { _CommandStartHostingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandStartHosting Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandStartHosting = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandStartHostingLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandStartHostingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandStartHosting), model => model.Initialize(nameof(CommandStartHosting), ref model._CommandStartHosting, ref _CommandStartHostingLocator, _CommandStartHostingDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandStartHostingDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandStartHosting);           // Command resource  
                var commandId = nameof(CommandStartHosting);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add StartHosting logic here, or
                            vm.CurrentTask.Start();
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




        public CommandModel<ReactiveCommand, String> CommandStopHosting
        {
            get { return _CommandStopHostingLocator(this).Value; }
            set { _CommandStopHostingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandStopHosting Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandStopHosting = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandStopHostingLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandStopHostingLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandStopHosting), model => model.Initialize(nameof(CommandStopHosting), ref model._CommandStopHosting, ref _CommandStopHostingLocator, _CommandStopHostingDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandStopHostingDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandStopHosting);           // Command resource  
                var commandId = nameof(CommandStopHosting);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: false) { ViewModel = model }; //New Command Core
               cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            vm.CurrentTask.Stop();
                            //Todo: Add StopHosting logic here, or
                            await MVVMSidekick.Utilities.TaskExHelper.Yield();
                        })
                    .DoNotifyDefaultEventRouter(vm, commandId)
                    .Subscribe()
                    .DisposeWith(vm);

                var cmdmdl = cmd.CreateCommandModel(resource);

                var ob = vm.ListenChanged(x => x.IsUIBusy)
                   .Select(x => vm.IsUIBusy);
                cmd.ListenCanExecuteObservable(ob);

                return cmdmdl;
            };

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

                cmdmdl.ListenToIsUIBusy(
                    model: vm,
                    canExecuteWhenBusy: false);
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
                            //Todo: Add NewHost logic here, or
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

