using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    //[DataContract() ] //if you want
    public class ContentTypePair : BindableBase<ContentTypePair>
    {


        public string ExtensionName
        {
            get { return _ExtensionNameLocator(this).Value; }
            set { _ExtensionNameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ExtensionName Setup        
        protected Property<string> _ExtensionName = new Property<string> { LocatorFunc = _ExtensionNameLocator };
        static Func<BindableBase, ValueContainer<string>> _ExtensionNameLocator = RegisterContainerLocator<string>(nameof(ExtensionName), model => model.Initialize(nameof(ExtensionName), ref model._ExtensionName, ref _ExtensionNameLocator, _ExtensionNameDefaultValueFactory));
        static Func<string> _ExtensionNameDefaultValueFactory = () => default(string);
        #endregion


        public string ContentType
        {
            get { return _ContentTypeLocator(this).Value; }
            set { _ContentTypeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ContentType Setup        
        protected Property<string> _ContentType = new Property<string> { LocatorFunc = _ContentTypeLocator };
        static Func<BindableBase, ValueContainer<string>> _ContentTypeLocator = RegisterContainerLocator<string>(nameof(ContentType), model => model.Initialize(nameof(ContentType), ref model._ContentType, ref _ContentTypeLocator, _ContentTypeDefaultValueFactory));
        static Func<string> _ContentTypeDefaultValueFactory = () => default(string);
        #endregion


    }





}
