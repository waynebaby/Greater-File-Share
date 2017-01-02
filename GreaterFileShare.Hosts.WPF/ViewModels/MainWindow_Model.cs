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
            if (IsInDesignMode)
            {

            }

        }




        public ShareFileTask CurrentShareFileTask
        {
            get { return _CurrentShareFileTaskLocator(this).Value; }
            set { _CurrentShareFileTaskLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ShareFileTask CurrentShareFileTask Setup        
        protected Property<ShareFileTask> _CurrentShareFileTask = new Property<ShareFileTask> { LocatorFunc = _CurrentShareFileTaskLocator };
        static Func<BindableBase, ValueContainer<ShareFileTask>> _CurrentShareFileTaskLocator = RegisterContainerLocator<ShareFileTask>(nameof(CurrentShareFileTask), model => model.Initialize(nameof(CurrentShareFileTask), ref model._CurrentShareFileTask, ref _CurrentShareFileTaskLocator, _CurrentShareFileTaskDefaultValueFactory));
        static Func<ShareFileTask> _CurrentShareFileTaskDefaultValueFactory = () => default(ShareFileTask);
        #endregion




        public EditOrAddState CurrentEditOrAddState
        {
            get { return _CurrentEditOrAddStateLocator(this).Value; }
            set { _CurrentEditOrAddStateLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property EditOrAddState CurrentEditOrAddState Setup        
        protected Property<EditOrAddState> _CurrentEditOrAddState = new Property<EditOrAddState> { LocatorFunc = _CurrentEditOrAddStateLocator };
        static Func<BindableBase, ValueContainer<EditOrAddState>> _CurrentEditOrAddStateLocator = RegisterContainerLocator<EditOrAddState>(nameof(CurrentEditOrAddState), model => model.Initialize(nameof(CurrentEditOrAddState), ref model._CurrentEditOrAddState, ref _CurrentEditOrAddStateLocator, _CurrentEditOrAddStateDefaultValueFactory));
        static Func<EditOrAddState> _CurrentEditOrAddStateDefaultValueFactory = () => default(EditOrAddState);
        #endregion



        public ObservableCollection<ShareFileTask> AllTasks
        {
            get { return _AllTasksLocator(this).Value; }
            set { _AllTasksLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ShareFileTask> AllTasks Setup        
        protected Property<ObservableCollection<ShareFileTask>> _AllTasks = new Property<ObservableCollection<ShareFileTask>> { LocatorFunc = _AllTasksLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ShareFileTask>>> _AllTasksLocator = RegisterContainerLocator<ObservableCollection<ShareFileTask>>(nameof(AllTasks), model => model.Initialize(nameof(AllTasks), ref model._AllTasks, ref _AllTasksLocator, _AllTasksDefaultValueFactory));
        static Func<ObservableCollection<ShareFileTask>> _AllTasksDefaultValueFactory = () => new ObservableCollection<ShareFileTask>();
        #endregion



        public CommandModel<ReactiveCommand, String> CommandInitAddingNew
        {
            get { return _CommandInitAddingNewLocator(this).Value; }
            set { _CommandInitAddingNewLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandInitAddingNew Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandInitAddingNew = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandInitAddingNewLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandInitAddingNewLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandInitAddingNew), model => model.Initialize(nameof(CommandInitAddingNew), ref model._CommandInitAddingNew, ref _CommandInitAddingNewLocator, _CommandInitAddingNewDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandInitAddingNewDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandInitAddingNew);           // Command resource  
                var commandId = nameof(CommandInitAddingNew);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add InitAddingNew logic here, or
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




        public CommandModel<ReactiveCommand, String> CommandMarkDeleteSelectedItem
        {
            get { return _CommandMarkDeleteSelectedItemLocator(this).Value; }
            set { _CommandMarkDeleteSelectedItemLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandMarkDeleteSelectedItem Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandMarkDeleteSelectedItem = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandMarkDeleteSelectedItemLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandMarkDeleteSelectedItemLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandMarkDeleteSelectedItem), model => model.Initialize(nameof(CommandMarkDeleteSelectedItem), ref model._CommandMarkDeleteSelectedItem, ref _CommandMarkDeleteSelectedItemLocator, _CommandMarkDeleteSelectedItemDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandMarkDeleteSelectedItemDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandMarkDeleteSelectedItem);           // Command resource  
                var commandId = nameof(CommandMarkDeleteSelectedItem);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                vm,
                async e =>
                        {
                        //Todo: Add MarkDeleteSelectedItem logic here, or
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



        public CommandModel<ReactiveCommand, String> CommandSaveConfiguration
        {
            get { return _CommandSaveConfigurationLocator(this).Value; }
            set { _CommandSaveConfigurationLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandSaveConfiguration Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandSaveConfiguration = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandSaveConfigurationLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandSaveConfigurationLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandSaveConfiguration), model => model.Initialize(nameof(CommandSaveConfiguration), ref model._CommandSaveConfiguration, ref _CommandSaveConfigurationLocator, _CommandSaveConfigurationDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandSaveConfigurationDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandSaveConfiguration);           // Command resource  
                var commandId = nameof(CommandSaveConfiguration);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            //Todo: Add SaveConfiguration logic here, or
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


    public enum EditOrAddState
    {
        Normal = 0,
        EditingOld,
        AddingNew


    }

}

