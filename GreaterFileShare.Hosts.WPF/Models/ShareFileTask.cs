using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GreaterFileShare.Hosts.Core;
using MVVMSidekick.Services;
using System.IO;
using MVVMSidekick.Reactive;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GreaterFileShare.Hosts.WPF.Models
{

    [DataContract()] //if you want
    public class ShareFileTask : BindableBase<ShareFileTask>
    {
        public ShareFileTask()
        {

            AddDisposeAction(() =>
            {
                if (IsHosting)
                {
                    Stop();
                }
            });
        }

        void SetupStarted(Task task, CancellationTokenSource cancelSource)
        {
            _task = task;
            _task?.ToObservable()
                .ObserveOnDispatcher()
                .Subscribe(
                    e => { },
                    e =>
                    {
                        if (!(e is TaskCanceledException))
                        {
                            IsLastStartFailed = true;
                            LastException = e;
                            GlobalEventRouter.RaiseEvent(this, e);
                        }
                        IsHosting = false;
                    },
                    () =>
                    {
                        IsHosting = false;
                    }
                );
            _cancelSource = cancelSource;
        }

        [DataMember]

        public ObservableCollection<ContentTypePair> AdditionalContentTypes
        {
            get { return _AdditionalContentTypesLocator(this).Value; }
            set { _AdditionalContentTypesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<ContentTypePair> AdditionalContentTypes Setup        
        protected Property<ObservableCollection<ContentTypePair>> _AdditionalContentTypes = new Property<ObservableCollection<ContentTypePair>> { LocatorFunc = _AdditionalContentTypesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<ContentTypePair>>> _AdditionalContentTypesLocator = RegisterContainerLocator<ObservableCollection<ContentTypePair>>(nameof(AdditionalContentTypes), model => model.Initialize(nameof(AdditionalContentTypes), ref model._AdditionalContentTypes, ref _AdditionalContentTypesLocator, _AdditionalContentTypesDefaultValueFactory));
        static Func<ObservableCollection<ContentTypePair>> _AdditionalContentTypesDefaultValueFactory =
            () => new ObservableCollection<ContentTypePair>()
                    {
                        new ContentTypePair
                        {
                            ExtensionName = ".mkv",
                            ContentType ="video/mkv"
                        }
            };


        #endregion



        public void Start()
        {
            Start(this.Path, this.Port.Value);
        }
        public void Start(string path, int port)
        {
            try
            {
                if (IsHosting)
                {
                    throw new InvalidProgramException("Still Running, need shutdown first");
                }
                var l = ServiceLocator.Instance.Resolve<ILauncher>();
                var cts = new CancellationTokenSource();

                var s = new Dictionary<string, string>();
                foreach (var item in this.AdditionalContentTypes)
                {
                    if (string.IsNullOrWhiteSpace(item.ExtensionName) || string.IsNullOrWhiteSpace(item.ContentType))
                    {
                        GlobalEventRouter.RaiseEvent(this, $"Hosting:\t\tContent Type entry Got Empty Field. Ignored. \r\n\t\t\t\t\t\t {{{nameof(item.ExtensionName)}:{item.ExtensionName ?? ""},{nameof(item.ContentType)}:{item.ContentType}}}", "Logging");
                    }
                    else if (s.ContainsKey(item.ExtensionName))
                    {
                        GlobalEventRouter.RaiseEvent(this, $"Hosting:\tContent Type entry Got Duplicate ExtensionName. Ignored.\r\n\t\t\t\t\t\t {{{nameof(item.ExtensionName)}:{item.ExtensionName ?? ""},{nameof(item.ContentType)}:{item.ContentType}}}", "Logging");
                   }
                    else
                        s.Add(item.ExtensionName, item.ContentType);
                    }
    
                var t = l.RunWebsiteAsync(
                    path, port,
                    s,
                    cts.Token);

                SetupStarted(t, cts);
                IsHosting = true;
                IsLastStartFailed = false;
            }
            catch (Exception ex)
            {
                this.LastException = ex;
                IsLastStartFailed = true;
                GlobalEventRouter.RaiseEvent(this, ex);

            }

        }
        public void Stop()
        {
            _cancelSource?.Cancel();
        }

        [DataMember]

        public string Path
        {
            get { return _PathLocator(this).Value; }
            set { _PathLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Path Setup        
        protected Property<string> _Path = new Property<string> { LocatorFunc = _PathLocator };
        static Func<BindableBase, ValueContainer<string>> _PathLocator = RegisterContainerLocator<string>(nameof(Path), model => model.Initialize(nameof(Path), ref model._Path, ref _PathLocator, _PathDefaultValueFactory));
        static Func<string> _PathDefaultValueFactory = () => "c:\\";// new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.GetDirectories("contents").FirstOrDefault()?.FullName;

        #endregion


        [DataMember]

        public int? Port
        {
            get { return _PortLocator(this).Value; }
            set { _PortLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int? Port Setup        
        protected Property<int?> _Port = new Property<int?> { LocatorFunc = _PortLocator };
        static Func<BindableBase, ValueContainer<int?>> _PortLocator = RegisterContainerLocator<int?>(nameof(Port), model => model.Initialize(nameof(Port), ref model._Port, ref _PortLocator, _PortDefaultValueFactory));
        static Func<int?> _PortDefaultValueFactory = () => 5000;
        #endregion


        Task _task;
        CancellationTokenSource _cancelSource;



        public bool IsHosting
        {
            get { return _IsHostingLocator(this).Value; }
            private set { _IsHostingLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsHosting Setup        
        protected Property<bool> _IsHosting = new Property<bool> { LocatorFunc = _IsHostingLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsHostingLocator = RegisterContainerLocator<bool>(nameof(IsHosting), model => model.Initialize(nameof(IsHosting), ref model._IsHosting, ref _IsHostingLocator, _IsHostingDefaultValueFactory));
        static Func<bool> _IsHostingDefaultValueFactory = () => default(bool);
        #endregion



        public Exception LastException
        {
            get { return _LastExceptionLocator(this).Value; }
            set { _LastExceptionLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property Exception LastException Setup        
        protected Property<Exception> _LastException = new Property<Exception> { LocatorFunc = _LastExceptionLocator };
        static Func<BindableBase, ValueContainer<Exception>> _LastExceptionLocator = RegisterContainerLocator<Exception>(nameof(LastException), model => model.Initialize(nameof(LastException), ref model._LastException, ref _LastExceptionLocator, _LastExceptionDefaultValueFactory));
        static Func<Exception> _LastExceptionDefaultValueFactory = () => default(Exception);
        #endregion


        public bool IsLastStartFailed
        {
            get { return _IsLastStartFailedLocator(this).Value; }
            set { _IsLastStartFailedLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property bool IsLastStartFailed Setup        
        protected Property<bool> _IsLastStartFailed = new Property<bool> { LocatorFunc = _IsLastStartFailedLocator };
        static Func<BindableBase, ValueContainer<bool>> _IsLastStartFailedLocator = RegisterContainerLocator<bool>(nameof(IsLastStartFailed), model => model.Initialize(nameof(IsLastStartFailed), ref model._IsLastStartFailed, ref _IsLastStartFailedLocator, _IsLastStartFailedDefaultValueFactory));
        static Func<bool> _IsLastStartFailedDefaultValueFactory = () => default(bool);
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

                cmd.Do(
                         e =>
                        {
                            //Todo: Add StartHosting logic here, or
                            if (!vm.IsHosting)
                            {
                                vm.Start();
                            }
                            else
                            {
                                vm.Stop();
                            }
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
