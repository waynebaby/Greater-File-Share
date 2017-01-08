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

namespace GreaterFileShare.Hosts.WPF.ViewModels
{

    public class UriAndQRs_Model : ViewModelBase<UriAndQRs_Model>
    {
        // If you have install the code sniplets, use "propvm + [tab] +[tab]" create a property propcmd for command
        // 如果您已经安装了 MVVMSidekick 代码片段，请用 propvm +tab +tab 输入属性 propcmd 输入命令

        public UriAndQRs_Model()
        {

 

        }


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



        protected override async Task OnBindedViewLoad(IView view)
        {
            var nh = ServiceLocator.Instance.Resolve<INetworkService>();
            if (CurrentTask==null)
            {
                throw new InvalidOperationException("need a CurrentTask instance first");
            }
            Hosts = new ObservableCollection<string>(nh.GetHosts().Select(x =>$"http://{x}:{CurrentTask.Port}/Files"));
            await  base.OnBindedViewLoad(view);
        }

    }

}

