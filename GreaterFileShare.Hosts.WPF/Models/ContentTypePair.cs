using MVVMSidekick.Reactive;
using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    [DataContract()] //if you want
    public class ContentTypePair : BindableBase<ContentTypePair>
    {
        //public ContentTypePair()
        //{


        //    InitCheckErrorEvent();

        //    ExtensionName = "";
        //    ContentType = "";

        //}
        //protected override void OnDeserializingActions()
        //{
        //    base.OnDeserializingActions();
        //    InitCheckErrorEvent();

        //}

        //private void InitCheckErrorEvent()
        //{
        //    var ct = this.GetValueContainer(x => x.ExtensionName);
        //    CheckEmpty(ct);
           
            
        //    ct = this.GetValueContainer(x => x.ContentType);
        //    CheckEmpty(ct);

            
        //}

        //private void CheckEmpty(ValueContainer<string> ct)
        //{
        //    ct.GetNullObservable()
        //         .Select(_ => String.IsNullOrEmpty(ct.Value))
        //         .Subscribe(
        //             v =>
        //             {

        //                 if (v)
        //                 {

        //                     ct.Errors.Add(new ErrorEntity { Message = $"{ct.PropertyName} Cannot Be Null or Empty" });
        //                 }
        //                 else
        //                 {
        //                     ct.Errors.Clear();
        //                 }
        //             }
        //         ).DisposeWith(this);
        //}

        [DataMember]
        public string ExtensionName
        {
            get { return _ExtensionNameLocator(this).Value ?? ""; }
            set { _ExtensionNameLocator(this).SetValueAndTryNotify(value ?? ""); }
        }
        #region Property string ExtensionName Setup        
        protected Property<string> _ExtensionName = new Property<string> { LocatorFunc = _ExtensionNameLocator };
        static Func<BindableBase, ValueContainer<string>> _ExtensionNameLocator = RegisterContainerLocator<string>(nameof(ExtensionName), model => model.Initialize(nameof(ExtensionName), ref model._ExtensionName, ref _ExtensionNameLocator, _ExtensionNameDefaultValueFactory));
        static Func<string> _ExtensionNameDefaultValueFactory = () => null;
        #endregion

        [DataMember]
        public string ContentType
        {
            get { return _ContentTypeLocator(this).Value ?? ""; }
            set { _ContentTypeLocator(this).SetValueAndTryNotify(value ?? ""); }
        }
        #region Property string ContentType Setup        
        protected Property<string> _ContentType = new Property<string> { LocatorFunc = _ContentTypeLocator };
        static Func<BindableBase, ValueContainer<string>> _ContentTypeLocator = RegisterContainerLocator<string>(nameof(ContentType), model => model.Initialize(nameof(ContentType), ref model._ContentType, ref _ContentTypeLocator, _ContentTypeDefaultValueFactory));
        static Func<string> _ContentTypeDefaultValueFactory = () => null;
        #endregion


    }





}
