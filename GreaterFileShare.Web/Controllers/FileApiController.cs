using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreaterFileShare.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GreaterFileShare.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileApiController : Controller
    {

        //public FileApiController()
        //{

        //}
        public FileApiController(IFileSystemService rootService)
        {
            _rootService = rootService;
        }
        IFileSystemService _rootService;


        [HttpGet(nameof(GetFiles) + "/{folderPath}")]

        public Task<IList<FileEntry>> GetFiles(string folderPath)
        {
            return _rootService.GetFilesAsync(folderPath);
        }


        [HttpGet(nameof(GetFolders) + "/{folderPath}")]

        public Task<IList<FolderEntry>> GetFolders(string folderPath)
        {
            return _rootService.GetFoldersAsync(folderPath);
        }


        [HttpGet(nameof(GetShortUriCache) + "/{shortUriKey}")]
        public Task<Entry> GetShortUriCache(string shortUriKey)
        {
            shortUriKey = System.Net.WebUtility.UrlDecode(shortUriKey);
            return _rootService.GetShortUriCacheAsync(shortUriKey);
        }



        [HttpGet(nameof(GetRootPath))]

        public Task<string> GetRootPath()
        {
            return _rootService.GetRootPathAsync();
        }
    }
}
