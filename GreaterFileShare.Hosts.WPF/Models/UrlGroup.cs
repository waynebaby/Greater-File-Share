using MVVMSidekick.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Hosts.WPF.Models
{

    //[DataContract() ] //if you want
    public class UrlGroup : BindableBase<UrlGroup>
    {
        public UrlGroup()
        {
        }

        public UrlGroup(string host, int port, FileEntry file, FolderEntry folder, ShareFileTask currentTask)
        {
            WCF = $"net.tcp://{host}:{Consts.WCFPort}/{Consts.WCFRelativeUri}";
            API = $"http://{host}:{port}/{Consts.SwaggerRelativeUri}";
            if (file == null && folder == null)
            {
                Files = $"http://{host}:{port}/{Consts.FilesRelativeUri}";

            }
            else if (file != null)
            {
                var rltvUrl = file
                    .FullPath
                    .Remove(0, currentTask.Path.Length)
                    .Replace('\\', '/')
                    .Trim('/');
                Files = $"http://{host}:{port}/{Consts.FilesRelativeUri}/{rltvUrl}";
            }
            else if (folder != null)
            {
                var rltvUrl = folder
                     .FullPath
                     .Remove(0, currentTask.Path.Length)
                     .Replace('\\', '/')
                     .Trim('/');
                Files = $"http://{host}:{port}/{Consts.FilesRelativeUri}/{rltvUrl}";
            }
        }

        public string WCF
        {
            get { return _WCFLocator(this).Value; }
            set { _WCFLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string WCF Setup        
        protected Property<string> _WCF = new Property<string> { LocatorFunc = _WCFLocator };
        static Func<BindableBase, ValueContainer<string>> _WCFLocator = RegisterContainerLocator<string>(nameof(WCF), model => model.Initialize(nameof(WCF), ref model._WCF, ref _WCFLocator, _WCFDefaultValueFactory));
        static Func<string> _WCFDefaultValueFactory = () => nameof(WCF);
        #endregion


        public string File
        {
            get { return _FileLocator(this).Value; }
            set { _FileLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string File Setup        
        protected Property<string> _File = new Property<string> { LocatorFunc = _FileLocator };
        static Func<BindableBase, ValueContainer<string>> _FileLocator = RegisterContainerLocator<string>(nameof(File), model => model.Initialize(nameof(File), ref model._File, ref _FileLocator, _FileDefaultValueFactory));
        static Func<string> _FileDefaultValueFactory = () => default(string);
        #endregion



        public string API
        {
            get { return _APILocator(this).Value; }
            set { _APILocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string API Setup        
        protected Property<string> _API = new Property<string> { LocatorFunc = _APILocator };
        static Func<BindableBase, ValueContainer<string>> _APILocator = RegisterContainerLocator<string>(nameof(API), model => model.Initialize(nameof(API), ref model._API, ref _APILocator, _APIDefaultValueFactory));
        static Func<string> _APIDefaultValueFactory = () => nameof(API);
        #endregion




        public string Files
        {
            get { return _FilesLocator(this).Value; }
            set { _FilesLocator(this).SetValueAndTryNotify(value); }
        }
        #region Property string Files Setup        
        protected Property<string> _Files = new Property<string> { LocatorFunc = _FilesLocator };
        static Func<BindableBase, ValueContainer<string>> _FilesLocator = RegisterContainerLocator<string>(nameof(Files), model => model.Initialize(nameof(Files), ref model._Files, ref _FilesLocator, _FilesDefaultValueFactory));
        static Func<string> _FilesDefaultValueFactory = () => nameof(Files);
        #endregion


    }






}
