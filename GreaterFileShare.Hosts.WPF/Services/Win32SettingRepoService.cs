using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Attributes;

namespace GreaterFileShare.Hosts.WPF.Services
{
    public class Win32SettingRepoService<T> : ISettingRepoService<T>
    {
        public Win32SettingRepoService([Dependency(nameof(SuggestedTargetFilePath))] string suggestedTargetFilePath)
        {
            SuggestedTargetFilePath = suggestedTargetFilePath;

        }
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
            using (var stm = file.Open(FileMode.Create))
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

            if (string.IsNullOrWhiteSpace(SuggestedTargetFilePath))
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
