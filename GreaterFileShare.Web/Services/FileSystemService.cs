
using GreaterFileShare.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreaterFileShare.Services
{
    public class FileSystemService : IFileSystemService
    {
        const int MaxCacheCount = 5000;

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

        public async Task<IList<FileEntry>> GetFilesAsync(string folderPath)
        {
            if (!folderPath.StartsWith(_taskRoot))
            {
                throw new InvalidOperationException("Leaving Allowed Area?");
            }

            var di = new DirectoryInfo(folderPath);
            var fis = di.GetFiles();
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {

                var rval = fis.Select(
                    fi =>
                    {
                        var fn = fi.FullName;
                        var diff = fn.Substring(_rootFolder.Value.FullPath.Length);
                        var uri = diff.Replace("\\", "/");
                        var relativeUri = $"{Consts.FilesRelativeUri}/{uri.Trim('/')}";
                        var bin = md5.ComputeHash(Encoding.UTF8.GetBytes(relativeUri));
                        var key = Encode(bin);
                        //var key = uri;
                        var fe = new FileEntry
                        {
                            FullPath = fi.FullName,
                            Name = fi.Name,
                            RelativeUri = relativeUri,
                            ShortUriKey = key
                        };
                        return fe;
                    });
                var entries = rval as IEnumerable<Entry>;

                await AddEntriesToCache(entries);

                return rval.ToList();
            }
        }

        private async Task AddEntriesToCache(IEnumerable<Entry> entries)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (var item in entries)
                {

                    Cache[item.ShortUriKey] = item;
                    CacheAliveQueue.Enqueue(item);
                }

                while (Cache.Count > MaxCacheCount)
                {
                    var target = CacheAliveQueue.Dequeue();
                    Cache.Remove(target.ShortUriKey);
                }
            },
            CancellationToken.None,
            TaskCreationOptions.None,
            cacheVisit.ExclusiveScheduler);
        }

        static System.Threading.Tasks.ConcurrentExclusiveSchedulerPair cacheVisit = new ConcurrentExclusiveSchedulerPair();
        static Dictionary<string, Entry> Cache = new Dictionary<string, Entry>();
        static Queue<Entry> CacheAliveQueue = new Queue<Entry>();

        public async Task<IList<FolderEntry>> GetFoldersAsync(string folderPath)
        {
            if (!folderPath.StartsWith(_taskRoot))
            {
                throw new InvalidOperationException("Leaving Allowed Area?");
            }
            var rdi = new DirectoryInfo(folderPath);
            var dis = rdi.GetDirectories();
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var rval = dis.Select(
                di =>
                {
                    var fn = di.FullName;
                    var diff = fn.Substring(_rootFolder.Value.FullPath.Length);
                    var uri = diff.Replace("\\", "/");
                    var relativeUri = $"{Consts.FilesRelativeUri}/{uri.Trim('/')}";
                    var bin = md5.ComputeHash(Encoding.UTF8.GetBytes(relativeUri));
                    var key = Encode(bin);
                    //var key = Guid.NewGuid().ToString ();
                    var fe = new FolderEntry
                    {
                        FullPath = di.FullName,
                        Name = di.Name,
                        RelativeUri = relativeUri,
                        ShortUriKey = key
                    };
                    return fe;
                });

                var entries = rval as IEnumerable<Entry>;

                await AddEntriesToCache(entries);


                return rval.ToList();
            }
        }

        public async Task<Entry> GetShortUriCacheAsync(string shortUriKey)
        {

            var entry = await Task.Factory.StartNew(() =>
             {
                 Cache.TryGetValue(shortUriKey, out var rval);
                 return rval;
             },
            CancellationToken.None,
            TaskCreationOptions.None,
            cacheVisit.ConcurrentScheduler);

            return entry;
        }

        String Encode( byte[] binary)
        {
          var map= "1234567890abcdefghijlkmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-.";
            Stack<char> resultSet = new Stack<char>();

            var e = binary.AsEnumerable<Byte>().GetEnumerator();
            if (!e.MoveNext())
            {
                return "";
            }     
            var currentByte = e.Current;

            void Dig(int currentValue)
            {
                var index = currentValue / map.Length;
                var remain = currentValue % map.Length;

                resultSet.Push(map[remain]);
                if (index < map.Length)
                {
                    if (e.MoveNext())
                    {
                        index = index *map.Length + e.Current ;
                    }
                    else
                    {
                        resultSet.Push(map[index]);
                        return;
                    }
                }
                Dig(index);
            }


            Dig(currentByte);


            return new string(resultSet.ToArray());

        }

        public async Task<string> GetRootPathAsync()
        {
            return _taskRoot;
        }
    }
}
