
using GreaterFileShare.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreaterFileShare.Services
{
    public class FileSystemService :IFileSystemService
    {


        public FileSystemService(string rootPath)
        {
            if (rootPath == null)
            {
                throw new ArgumentNullException(nameof(rootPath));
            }
            _taskRoot = rootPath;
            _rootFolder = new Lazy<FolderEntry>(
                () =>
                {
                    var fi = new DirectoryInfo(_taskRoot);
                    var e = new FolderEntry
                    {
                        Name = fi.Name,
                        FullPath = fi.FullName
                    };
                    return e;
                },
                true
            );
        }





        private string _taskRoot;
        private Lazy<FolderEntry> _rootFolder;

        public async Task<IList<FileEntry>> GetFilesAsync(FolderEntry folder)
        {
            var di = new DirectoryInfo(folder.FullPath);
            var fis = di.GetFiles();

            var rval = fis.Select(
                fi =>
                {
                    var fn = fi.FullName;
                    var diff = fn.Substring(_rootFolder.Value.FullPath.Length);
                    var uri = diff.Replace("\\", "/");
                    var fe = new FileEntry
                    {
                        FullPath = fi.FullName,
                        Name = fi.Name,
                        RelativeUri = $"{Consts.FilesRelativeUri}/{uri.Trim('/')}"
                    };
                    return fe;
                });

            return rval.ToList();
        }

        public async Task<IList<FolderEntry>> GetFoldersAsync(FolderEntry folder)
        {
            var rdi = new DirectoryInfo(folder.FullPath);
            var dis = rdi.GetDirectories();

            var rval = dis.Select(
                di =>
                {
                    var fn = di.FullName;
                    var diff = fn.Substring(_rootFolder.Value.FullPath.Length);
                    var uri = diff.Replace("\\", "/");
                    var fe = new FolderEntry
                    {
                        FullPath = di.FullName,
                        Name = di.Name,
                    };
                    return fe;
                });

            return rval.ToList();
        }


    }
}
