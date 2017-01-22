using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    //[DataContract() ] //if you want
    public class HostEntry : BindableBase<HostEntry>
    {
        public HostEntry()
        {
          


        }

        //Use propvm + tab +tab  to create a new property of bindable here:

        public string HostName
        {
            get { return _HostNameLocator(this).Value; }
            set { _HostNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string HostName Setup        
        protected Property<string> _HostName = new Property<string> { LocatorFunc = _HostNameLocator };
        static Func<BindableBase, ValueContainer<string>> _HostNameLocator = RegisterContainerLocator<string>(nameof(HostName), model => model.Initialize(nameof(HostName), ref model._HostName, ref _HostNameLocator, _HostNameDefaultValueFactory));
        static Func<string> _HostNameDefaultValueFactory = () => default(string);
        #endregion



        public string AdapterName
        {
            get { return _AdapterNameLocator(this).Value; }
            set { _AdapterNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string AdapterName Setup        
        protected Property<string> _AdapterName = new Property<string> { LocatorFunc = _AdapterNameLocator };
        static Func<BindableBase, ValueContainer<string>> _AdapterNameLocator = RegisterContainerLocator<string>(nameof(AdapterName), model => model.Initialize(nameof(AdapterName), ref model._AdapterName, ref _AdapterNameLocator, _AdapterNameDefaultValueFactory));
        static Func<string> _AdapterNameDefaultValueFactory = () => default(string);
        #endregion



        public override string ToString()
        {
            return $"{AdapterName}\t{HostName}";
        }

    }





}
