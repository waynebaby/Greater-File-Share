using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    //[DataContract() ] //if you want
    public class MessageEntry : BindableBase<MessageEntry>
    {


        public DateTime Time
        {
            get { return _TimeLocator(this).Value; }
            set { _TimeLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property DateTime Time Setup        
        protected Property<DateTime> _Time = new Property<DateTime> { LocatorFunc = _TimeLocator };
        static Func<BindableBase, ValueContainer<DateTime>> _TimeLocator = RegisterContainerLocator<DateTime>(nameof(Time), model => model.Initialize(nameof(Time), ref model._Time, ref _TimeLocator, _TimeDefaultValueFactory));
        static Func<DateTime> _TimeDefaultValueFactory = () => default(DateTime);
        #endregion



        public String Message
        {
            get { return _MessageLocator(this).Value; }
            set { _MessageLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property String Message Setup        
        protected Property<String> _Message = new Property<String> { LocatorFunc = _MessageLocator };
        static Func<BindableBase, ValueContainer<String>> _MessageLocator = RegisterContainerLocator<String>(nameof(Message), model => model.Initialize(nameof(Message), ref model._Message, ref _MessageLocator, _MessageDefaultValueFactory));
        static Func<String> _MessageDefaultValueFactory = () => default(String);
        #endregion

    }





}
