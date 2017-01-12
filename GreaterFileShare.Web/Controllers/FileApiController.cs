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
    public class FileApiController : Controller, IFileSystemService
    {

        public FileApiController()
        {

        }
        public FileApiController(IFileSystemService rootService)
        {
            _rootService = rootService;
        }
        IFileSystemService _rootService;


        [HttpGet(nameof(GetFilesAsync))]

        public Task<IList<FileEntry>> GetFilesAsync(FolderEntry folder)
        {
            return _rootService.GetFilesAsync(folder);
        }


        [HttpGet(nameof(GetFoldersAsync))]

        public Task<IList<FolderEntry>> GetFoldersAsync(FolderEntry folder)
        {
            return _rootService.GetFoldersAsync(folder);
        }
    }
}
