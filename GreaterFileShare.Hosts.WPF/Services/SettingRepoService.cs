using MVVMSidekick.EventRouting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Attributes;
using Windows.Storage;

namespace GreaterFileShare.Hosts.WPF.Services
{


    public class SettingRepoService<T> : ISettingRepoService<T>
    {
        public SettingRepoService([Dependency(nameof(SuggestedTargetStorageItem))] IStorageItem suggestedTargetStorageItem)
        {
            SuggestedTargetStorageItem = suggestedTargetStorageItem;
        }
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
            IStorageFolder folder = null;
            var suggestted = SuggestedTargetStorageItem;
            StorageFile file = suggestted as StorageFile;


            var surffix = string.IsNullOrEmpty(Name) ? "Default" : Name;
            var fileName = $"Setting_{surffix}{Consts.SettingExtension}";


            if (file == null)
            {

                try
                {
                    var fds = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Documents);
                    folder = SuggestedTargetStorageItem as IStorageFolder;
                    folder = folder ?? fds.SaveFolder;
                }
                catch (InvalidOperationException)
                {
                    folder = folder ?? KnownFolders.DocumentsLibrary;
                }



                file = await folder.GetFileAsync(fileName);
            }

            if (createNew && file != null)
            {
                await file.DeleteAsync();
                file = null;
            }

            if (file == null || createNew)
            {

                file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                EventRouter.Instance.RaiseEvent(this, $"targeting file\t{file.Path}", "Logging");
                return file;
            }

           
           
            return file;
        }


    }
}

