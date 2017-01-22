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
using System.IO;

namespace GreaterFileShare.Hosts.WPF.ViewModels
{

    public class UriAndQRs_Model : ViewModelBase<UriAndQRs_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令


        IFileSystemHubService _fileSystemHubService;

        public UriAndQRs_Model()
        {


        }

        //public UriAndQRs_Model(IFileSystemHubService fileSystemHubService, GreaterFileShare.Services.IFileSystemService fileSystemService)
        //{

        //}

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



        public ObservableCollection<HostEntry> Hosts
        {
            get { return _HostsLocator(this).Value; }
            set { _HostsLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<HostEntry> Hosts Setup        
        protected Property<ObservableCollection<HostEntry>> _Hosts = new Property<ObservableCollection<HostEntry>> { LocatorFunc = _HostsLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<HostEntry>>> _HostsLocator = RegisterContainerLocator<ObservableCollection<HostEntry>>(nameof(Hosts), model => model.Initialize(nameof(Hosts), ref model._Hosts, ref _HostsLocator, _HostsDefaultValueFactory));
        static Func<ObservableCollection<HostEntry>> _HostsDefaultValueFactory = () => default(ObservableCollection<HostEntry>);
        #endregion


        public ObservableCollection<FolderEntry> RootEntry
        {
            get { return _RootEntryLocator(this).Value; }
            set { _RootEntryLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<FolderEntry> RootEntry Setup        
        protected Property<ObservableCollection<FolderEntry>> _RootEntry = new Property<ObservableCollection<FolderEntry>> { LocatorFunc = _RootEntryLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<FolderEntry>>> _RootEntryLocator = RegisterContainerLocator<ObservableCollection<FolderEntry>>("RootEntry", model => model.Initialize("RootEntry", ref model._RootEntry, ref _RootEntryLocator, _RootEntryDefaultValueFactory));
        static Func<ObservableCollection<FolderEntry>> _RootEntryDefaultValueFactory = () => new ObservableCollection<FolderEntry>();
        #endregion




        public HostEntry SelectedHost
        {
            get { return _SelectedHostLocator(this).Value; }
            set { _SelectedHostLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property HostEntry SelectedHost Setup        
        protected Property<HostEntry> _SelectedHost = new Property<HostEntry> { LocatorFunc = _SelectedHostLocator };
        static Func<BindableBase, ValueContainer<HostEntry>> _SelectedHostLocator = RegisterContainerLocator<HostEntry>(nameof(SelectedHost), model => model.Initialize(nameof(SelectedHost), ref model._SelectedHost, ref _SelectedHostLocator, _SelectedHostDefaultValueFactory));
        static Func<HostEntry> _SelectedHostDefaultValueFactory = () => default(HostEntry);
        #endregion







        public FileEntry SelectedFileEntry
        {
            get { return _SelectedFileEntryLocator(this).Value; }
            set { _SelectedFileEntryLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property FileEntry SelectedFileEntry Setup        
        protected Property<FileEntry> _SelectedFileEntry = new Property<FileEntry> { LocatorFunc = _SelectedFileEntryLocator };
        static Func<BindableBase, ValueContainer<FileEntry>> _SelectedFileEntryLocator = RegisterContainerLocator<FileEntry>(nameof(SelectedFileEntry), model => model.Initialize(nameof(SelectedFileEntry), ref model._SelectedFileEntry, ref _SelectedFileEntryLocator, _SelectedFileEntryDefaultValueFactory));
        static Func<FileEntry> _SelectedFileEntryDefaultValueFactory = () => default(FileEntry);
        #endregion


        public FolderEntry SelectedFolderEntry
        {
            get { return _SelectedFolderEntryLocator(this).Value; }
            set { _SelectedFolderEntryLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property FolderEntry SelectedFolderEntry Setup        
        protected Property<FolderEntry> _SelectedFolderEntry = new Property<FolderEntry> { LocatorFunc = _SelectedFolderEntryLocator };
        static Func<BindableBase, ValueContainer<FolderEntry>> _SelectedFolderEntryLocator = RegisterContainerLocator<FolderEntry>(nameof(SelectedFolderEntry), model => model.Initialize(nameof(SelectedFolderEntry), ref model._SelectedFolderEntry, ref _SelectedFolderEntryLocator, _SelectedFolderEntryDefaultValueFactory));
        static Func<FolderEntry> _SelectedFolderEntryDefaultValueFactory = () => default(FolderEntry);
        #endregion



        protected override async Task OnBindedViewLoad(IView view)
        {
            this.ListenChanged(
                x => x.CurrentTask,
                x => x.SelectedHost,
                x => x.SelectedFileEntry,
                x => x.SelectedFolderEntry)
                .ObserveOnDispatcher()
                .Do(e =>
                {
                    Urls =
                        new Models.UrlGroup(
                            SelectedHost?.HostName ?? "localhost",
                            CurrentTask?.Port ?? 80,
                            SelectedFileEntry,
                            SelectedFolderEntry,
                            CurrentTask);
                })
               .Subscribe()
               .DisposeWhenUnload(this);
            ;


            if (!IsInDesignMode)
            {
                _fileSystemHubService = ServiceLocator.Instance.Resolve<IFileSystemHubService>();
            }

            var nh = ServiceLocator.Instance.Resolve<INetworkService>();
            if (CurrentTask == null)
            {
                throw new InvalidOperationException("need a CurrentTask instance first");
            }
            Hosts = new ObservableCollection<HostEntry>(nh.GetHosts());

            var foldere = new FolderEntry()
            {
                FullPath = CurrentTask.Path,
                Name = Path.GetFileNameWithoutExtension(CurrentTask.Path)
            };
            SelectedFolderEntry = foldere;
            RootEntry = new ObservableCollection<Models.FolderEntry> { foldere };
            SelectedFolderEntry = foldere;
            CommandFillCurrentFolder.Execute(null);
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
                            if (e.EventArgs.Parameter is string)
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




        public CommandModel<ReactiveCommand, String> CommandFillCurrentFolder
        {
            get { return _CommandFillCurrentFolderLocator(this).Value; }
            set { _CommandFillCurrentFolderLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property CommandModel<ReactiveCommand, String> CommandFillCurrentFolder Setup        

        protected Property<CommandModel<ReactiveCommand, String>> _CommandFillCurrentFolder = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandFillCurrentFolderLocator };
        static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandFillCurrentFolderLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandFillCurrentFolder), model => model.Initialize(nameof(CommandFillCurrentFolder), ref model._CommandFillCurrentFolder, ref _CommandFillCurrentFolderLocator, _CommandFillCurrentFolderDefaultValueFactory));
        static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandFillCurrentFolderDefaultValueFactory =
            model =>
            {
                var resource = nameof(CommandFillCurrentFolder);           // Command resource  
                var commandId = nameof(CommandFillCurrentFolder);
                var vm = CastToCurrentType(model);
                var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

                cmd.DoExecuteUIBusyTask(
                        vm,
                        async e =>
                        {
                            var _fileSystemService = ServiceLocator.Instance.ResolveFactory<GreaterFileShare.Services.IFileSystemService>(null, vm.CurrentTask.Path);

                            var targetF = vm.SelectedFolderEntry;
                            var foldert = _fileSystemService.GetFoldersAsync(targetF.FullPath);
                            var filet = _fileSystemService.GetFilesAsync(targetF.FullPath);

                            var folders = await foldert;
                            var files = await filet;

                            targetF.SubFolders = new ObservableCollection<FolderEntry>(
                                folders.Select(x => new FolderEntry { FullPath = x.FullPath, Name = x.Name }));
                            targetF.Files = new ObservableCollection<FileEntry>(
                                files.Select(x => new FileEntry { FullPath = x.FullPath, Name = x.Name }));

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




        //public CommandModel<ReactiveCommand, String> CommandChangeCurrentFile
        //{
        //    get { return _CommandChangeCurrentFileLocator(this).Value; }
        //    set { _CommandChangeCurrentFileLocator(this).SetValueAndTryNotify(value); }
        //}
        //#region Property CommandModel<ReactiveCommand, String> CommandChangeCurrentFile Setup        

        //protected Property<CommandModel<ReactiveCommand, String>> _CommandChangeCurrentFile = new Property<CommandModel<ReactiveCommand, String>> { LocatorFunc = _CommandChangeCurrentFileLocator };
        //static Func<BindableBase, ValueContainer<CommandModel<ReactiveCommand, String>>> _CommandChangeCurrentFileLocator = RegisterContainerLocator<CommandModel<ReactiveCommand, String>>(nameof(CommandChangeCurrentFile), model => model.Initialize(nameof(CommandChangeCurrentFile), ref model._CommandChangeCurrentFile, ref _CommandChangeCurrentFileLocator, _CommandChangeCurrentFileDefaultValueFactory));
        //static Func<BindableBase, CommandModel<ReactiveCommand, String>> _CommandChangeCurrentFileDefaultValueFactory =
        //    model =>
        //    {
        //        var resource = nameof(CommandChangeCurrentFile);           // Command resource  
        //        var commandId = nameof(CommandChangeCurrentFile);
        //        var vm = CastToCurrentType(model);
        //        var cmd = new ReactiveCommand(canExecute: true) { ViewModel = model }; //New Command Core

        //        cmd.DoExecuteUIBusyTask(
        //                vm,
        //                async e =>
        //                {
        //                    //Todo: Add ChangeCurrentFile logic here, or
        //                    await MVVMSidekick.Utilities.TaskExHelper.Yield();
        //                })
        //            .DoNotifyDefaultEventRouter(vm, commandId)
        //            .Subscribe()
        //            .DisposeWith(vm);

        //        var cmdmdl = cmd.CreateCommandModel(resource);

        //        cmdmdl.ListenToIsUIBusy(
        //            model: vm,
        //            canExecuteWhenBusy: false);
        //        return cmdmdl;
        //    };

        //#endregion


    }

}

