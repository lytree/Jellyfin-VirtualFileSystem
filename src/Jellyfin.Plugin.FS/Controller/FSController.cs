using Jellyfin.Plugin.FS.Configuration;
using ManagedCode.Storage.VirtualFileSystem.Core;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.FS.Controller
{
    [ApiController]
    [Route("fs")]
    public class FSController : ControllerBase
    {

        private readonly IVirtualFileSystem _vfs;

        public FSController(IVirtualFileSystem virtualFileSystem)
        {
            _vfs = virtualFileSystem;
        }


        [HttpGet("list/{*path}")]
        public ActionResult List(string path)
        {
            return Ok(_vfs.GetDirectoryAsync(path));
        }
    }
}
