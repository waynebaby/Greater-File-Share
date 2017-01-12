using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreaterFileShare.Services
{
    public interface IFileSystemService
    {
        Task<IList<FileEntry>> GetFilesAsync(FolderEntry folder);
        Task<IList<FolderEntry>> GetFoldersAsync(FolderEntry folder);
    }
}