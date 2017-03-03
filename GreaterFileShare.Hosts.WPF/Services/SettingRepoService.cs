using MVVMSidekick.EventRouting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GreaterFileShare.Hosts.WPF.Services
{

    public class SettingRepoService<T> : ISettingRepoService<T>
    {
        System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
        public string Name
        {
            get; set;
        }

        public IStorageItem SuggestedTargetStorageItem { get; set; }


        public async Task<T> LoadAsync()
        {
            var ms = new MemoryStream();

            var file = await GetTargetFileAsync();
            using (var stm = await file.OpenReadAsync())
            {
                var s2 = stm.AsStreamForRead();
                await s2.CopyToAsync(ms);
                ms.Position = 0;
            }
            var o = dcs.ReadObject(ms);

            if (o is T)
            {
                return (T)o;
            }
            return default(T);

        }

        public async Task SaveAsync(T entry)
        {
            var ms = new MemoryStream();
            dcs.WriteObject(ms, entry);
            ms.Position = 0;
            var file = await GetTargetFileAsync(true);
            using (var stm = await file.OpenTransactedWriteAsync(StorageOpenOptions.AllowOnlyReaders))
            {
                var s2 = stm.Stream.AsStreamForWrite();
                await ms.CopyToAsync(s2);
                await s2.FlushAsync();
                await stm.CommitAsync();
            }

        }


        async Task<StorageFile> GetTargetFileAsync(bool createNew = false)
        {
            StorageFile file = SuggestedTargetStorageItem as StorageFile;
            if (file != null)
            {
                return file;
            }

            IStorageFolder folder = SuggestedTargetStorageItem as IStorageFolder;
            try
            {
                var fds = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
                folder = folder ?? fds.SaveFolder;
            }
            catch (InvalidOperationException)
            {
                folder = folder ?? KnownFolders.DocumentsLibrary;
            }


            var surffix = string.IsNullOrEmpty(Name) ? "Default" : Name;
            var fileName = $"Setting_{surffix}{Consts.SettingExtension}";

            if (createNew)
            {

                file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                EventRouter.Instance.RaiseEvent(this, $"targeting file\t{file.Path}", "Logging");
                return file;
            }
            else
            {

                file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                EventRouter.Instance.RaiseEvent(this, $"targeting file\t{file.Path}", "Logging");
                return file;
            }


        }
    }
    public class Win32SettingRepoService<T> : ISettingRepoService<T>
    {
        System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
        public string Name
        {
            get; set;
        }

        public string SuggestedTargetFilePath { get; set; }


        public async Task<T> LoadAsync()
        {
            var ms = new MemoryStream();

            var file = await GetTargetFileAsync();
            using (var stm = file.OpenRead())
            {

                await stm.CopyToAsync(ms);
                ms.Position = 0;
            }
            var o = dcs.ReadObject(ms);

            if (o is T)
            {
                return (T)o;
            }
            return default(T);

        }

        public async Task SaveAsync(T entry)
        {
            var ms = new MemoryStream();
            dcs.WriteObject(ms, entry);
            ms.Position = 0;
            var file = await GetTargetFileAsync(true);
            using (var stm = file.Open( FileMode.Create))
            {
                var s2 = stm;
                await ms.CopyToAsync(s2);
                await s2.FlushAsync();
                s2.Close();
            }

        }


        async Task<FileInfo> GetTargetFileAsync(bool createNew = false)
        {
            bool isDir = true;

            if (SuggestedTargetFilePath != null)
            {
                isDir = false;
                if (!SuggestedTargetFilePath.EndsWith(Consts.SettingExtension))
                {
                    SuggestedTargetFilePath = SuggestedTargetFilePath + Consts.SettingExtension;
                }
            }
            var file = new FileInfo(SuggestedTargetFilePath);
            if (file.Exists)
            {
               
                return file;
            }
            else
            {

                file.Create();
                return file;
            }




        }



    }
}
