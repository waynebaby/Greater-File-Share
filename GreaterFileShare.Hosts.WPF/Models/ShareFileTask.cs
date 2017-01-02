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

namespace GreaterFileShare.Hosts.WPF.Models
{

    //[DataContract() ] //if you want
    public class ShareFileTask : BindableBase<ShareFileTask>
    {

        void SetupStarted(Task task, CancellationTokenSource cancelSource)
        {
            _task = task;
            _task?.ToObservable()
                .ObserveOnDispatcher()
                .Subscribe(
                    e=>{ },
                    e =>
                    {
                        IsHosting = false;
                    },
                    CancellationToken.None
                );
            _cancelSource = cancelSource;
        }
        public void Start(string path, int port)
        {
            if (IsHosting)
            {
                throw new InvalidProgramException("Still Running, need shutdown first");
            }
            var l = new Launcher();
            var cts = new CancellationTokenSource();
            var t = l.RunWebsiteAsync(path, port, cts.Token);
            SetupStarted(t, cts);
        }
        public void Stop()
        {
            _cancelSource?.Cancel();
        }

        public string Path
        {
            get { return _PathLocator(this).Value; }
            private set { _PathLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Path Setup        
        protected Property<string> _Path = new Property<string> { LocatorFunc = _PathLocator };
        static Func<BindableBase, ValueContainer<string>> _PathLocator = RegisterContainerLocator<string>(nameof(Path), model => model.Initialize(nameof(Path), ref model._Path, ref _PathLocator, _PathDefaultValueFactory));
        static Func<string> _PathDefaultValueFactory = () => "c:\\";
        #endregion


        public int Port
        {
            get { return _PortLocator(this).Value; }
            private set { _PortLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property int Port Setup        
        protected Property<int> _Port = new Property<int> { LocatorFunc = _PortLocator };
        static Func<BindableBase, ValueContainer<int>> _PortLocator = RegisterContainerLocator<int>(nameof(Port), model => model.Initialize(nameof(Port), ref model._Port, ref _PortLocator, _PortDefaultValueFactory));
        static Func<int> _PortDefaultValueFactory = () => 5000;
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



    }





}
