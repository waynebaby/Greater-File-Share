using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GreaterFileShare.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreaterFileShare.Web.Controllers
{
    [Route(Consts.ShortUrlRelativeUri)]
    public class ShortUriController : Controller
    {
        public ShortUriController(IFileSystemService rootService)
        {
            _rootService = rootService;
        }
        IFileSystemService _rootService;


        [HttpGet("{shortUriKey}")]
        public async Task<ActionResult> GetShortUri(string shortUriKey)
        {
            shortUriKey = System.Net.WebUtility.UrlDecode(shortUriKey);
            var entry = await _rootService.GetShortUriCacheAsync(shortUriKey);

            var enc = entry.RelativeUri.Split('/').Select(x => System.Net.WebUtility.UrlEncode(x));
            return Redirect("/" + string.Join("/", enc).Replace("+", "%20"));

        }
    }
}
