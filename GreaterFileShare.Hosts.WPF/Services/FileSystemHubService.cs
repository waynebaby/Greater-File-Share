using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreaterFileShare.Services;
using System.ServiceModel;
using GreaterFileShare.Hosts.WPF.Models;
using System.Collections.ObjectModel;
using GreaterFileShare.Hosts.WPF.ViewModels;
using Windows.Storage;
using System.IO;

namespace GreaterFileShare.Hosts.WPF.Services
{

    [ServiceBehavior(InstanceContextMode = System.ServiceModel.InstanceContextMode.Single)]
    public class FileSystemHubService : IFileSystemHubService
    {

        internal static MainWindow_Model vmInstance;

        public async Task<string> GetDefaultFolderAsync()
        {
            var fds = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Videos);
            var folder = fds.SaveFolder;
            return folder.Path;

            ////Need vlib cap and run in UWP process
            //var folder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.VideosLibrary);
            //var ancherFile = await folder.CreateFileAsync("_._", CreationCollisionOption.OpenIfExists);

            //var f2 = await ancherFile.GetParentAsync();
            //return f2.Path;
        }

        public async Task<IList<GreaterFileShare.Services.FileEntry>> GetFilesAsync(int port, string filePath)
        {
            var tsk = await vmInstance.ExecuteTask(
                async () =>
                    await Task.FromResult(vmInstance?.HostingTasks?.FirstOrDefault(x => x.Port == port && x.IsHosting)));

            if (tsk == null)
            {
                throw new IndexOutOfRangeException("No path here");
            }
            var s = new FileSystemService(tsk.Path);

            return await s.GetFilesAsync(filePath);

        }

        public async Task<IList<GreaterFileShare.Services.FolderEntry>> GetFoldersAsync(int port, string folderPath)
        {
            var tsk = await vmInstance.ExecuteTask(
             async () =>
                 await Task.FromResult(vmInstance?.HostingTasks?.FirstOrDefault(x => x.Port == port && x.IsHosting)));

            if (tsk == null)
            {
                throw new IndexOutOfRangeException("No path here");
            }
            var s = new FileSystemService(tsk.Path);

            return await s.GetFoldersAsync(folderPath);
        }
    }
}
