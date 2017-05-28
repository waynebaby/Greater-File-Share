using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreaterFileShare.Services
{
    public interface IFileSystemService
    {
        Task<IList<FileEntry>> GetFilesAsync(string folderPath);
        Task<IList<FolderEntry>> GetFoldersAsync(string folderPath);
        Task<string> GetRootPathAsync();
        Task<Entry> GetShortUriCacheAsync(string shortUriKey);
    }
}