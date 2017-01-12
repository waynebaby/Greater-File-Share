using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    [DataContract]
    public abstract class FileFolderEntry<TSub>:BindableBase<TSub> where  TSub: FileFolderEntry<TSub>
    {


        [DataMember]
        public string Name
        {
            get { return _NameLocator(this).Value; }
            set { _NameLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Name Setup        
        protected Property<string> _Name = new Property<string> { LocatorFunc = _NameLocator };
        static Func<BindableBase, ValueContainer<string>> _NameLocator = RegisterContainerLocator<string>(nameof(Name), model => model.Initialize(nameof(Name), ref model._Name, ref _NameLocator, _NameDefaultValueFactory));
        static Func<string> _NameDefaultValueFactory = () => default(string);
        #endregion


        [DataMember]

        public string FullPath
        {
            get { return _FullPathLocator(this).Value; }
            set { _FullPathLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string FullPath Setup        
        protected Property<string> _FullPath = new Property<string> { LocatorFunc = _FullPathLocator };
        static Func<BindableBase, ValueContainer<string>> _FullPathLocator = RegisterContainerLocator<string>(nameof(FullPath), model => model.Initialize(nameof(FullPath), ref model._FullPath, ref _FullPathLocator, _FullPathDefaultValueFactory));
        static Func<string> _FullPathDefaultValueFactory = () => default(string);
        #endregion



    }

    [DataContract]
    public class FileEntry : FileFolderEntry<FileEntry>
    {
        [DataMember]

        public string RelativeUri
        {
            get { return _RelativeUriLocator(this).Value; }
            set { _RelativeUriLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string RelativeUri Setup        
        protected Property<string> _RelativeUri = new Property<string> { LocatorFunc = _RelativeUriLocator };
        static Func<BindableBase, ValueContainer<string>> _RelativeUriLocator = RegisterContainerLocator<string>(nameof(RelativeUri), model => model.Initialize(nameof(RelativeUri), ref model._RelativeUri, ref _RelativeUriLocator, _RelativeUriDefaultValueFactory));
        static Func<string> _RelativeUriDefaultValueFactory = () => default(string);
        #endregion


    }

    [DataContract]
    public class FolderEntry : FileFolderEntry<FolderEntry>
    {

    }
}
