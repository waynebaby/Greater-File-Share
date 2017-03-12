using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    [DataContract]
    public abstract class FileFolderEntry<TSub> : BindableBase<TSub> where TSub : FileFolderEntry<TSub>
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

        [DataMember]

        public string ShortUriKey
        {
            get { return _ShortUriKeyLocator(this).Value; }
            set { _ShortUriKeyLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string ShortUriKey Setup        
        protected Property<string> _ShortUriKey = new Property<string> { LocatorFunc = _ShortUriKeyLocator };
        static Func<BindableBase, ValueContainer<string>> _ShortUriKeyLocator = RegisterContainerLocator<string>(nameof(ShortUriKey), model => model.Initialize(nameof(ShortUriKey), ref model._ShortUriKey, ref _ShortUriKeyLocator, _ShortUriKeyDefaultValueFactory));
        static Func<string> _ShortUriKeyDefaultValueFactory = () => default(string);
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
        [DataMember]
        public ObservableCollection<FolderEntry> SubFolders
        {
            get { return _SubFoldersLocator(this).Value; }
            set { _SubFoldersLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<FolderEntry> SubFolders Setup        
        protected Property<ObservableCollection<FolderEntry>> _SubFolders = new Property<ObservableCollection<FolderEntry>> { LocatorFunc = _SubFoldersLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<FolderEntry>>> _SubFoldersLocator = RegisterContainerLocator<ObservableCollection<FolderEntry>>(nameof(SubFolders), model => model.Initialize(nameof(SubFolders), ref model._SubFolders, ref _SubFoldersLocator, _SubFoldersDefaultValueFactory));
        static Func<ObservableCollection<FolderEntry>> _SubFoldersDefaultValueFactory = () => new ObservableCollection<FolderEntry>();
        #endregion


        public ObservableCollection<FileEntry> Files
        {
            get { return _FilesLocator(this).Value; }
            set { _FilesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property ObservableCollection<FileEntry> Files Setup        
        protected Property<ObservableCollection<FileEntry>> _Files = new Property<ObservableCollection<FileEntry>> { LocatorFunc = _FilesLocator };
        static Func<BindableBase, ValueContainer<ObservableCollection<FileEntry>>> _FilesLocator = RegisterContainerLocator<ObservableCollection<FileEntry>>(nameof(Files), model => model.Initialize(nameof(Files), ref model._Files, ref _FilesLocator, _FilesDefaultValueFactory));
        static Func<ObservableCollection<FileEntry>> _FilesDefaultValueFactory = () => new ObservableCollection<FileEntry>();
        #endregion


    }
}
