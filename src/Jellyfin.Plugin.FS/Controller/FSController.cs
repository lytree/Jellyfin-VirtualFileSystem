using Jellyfin.Plugin.FS.Configuration;
using ManagedCode.Storage.Core;
using ManagedCode.Storage.Server.ChunkUpload;
using ManagedCode.Storage.Server.Controllers;
using ManagedCode.Storage.VirtualFileSystem.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jellyfin.Plugin.FS.Controller
{
    [ApiController]
    [Route("fs/api/storage")]
    public class FSController : StorageControllerBase<IStorage>
    {
        private ILogger<FSController> _logger;
        /// <summary>
        /// Initialises a new instance of the default storage controller.
        /// </summary>
        /// <param name="storage">The shared storage instance.</param>
        /// <param name="chunkUploadService">Chunk upload coordinator.</param>
        /// <param name="options">Server behaviour options.</param>
        public FSController(
            IStorage storage,
            ChunkUploadService chunkUploadService,
            StorageServerOptions options, ILogger<FSController> logger) : base(storage, chunkUploadService, options)
        {
            _logger = logger;
        }
        [Route("init")]
        public IActionResult Init()
        {
            _logger.LogInformation("FSController Init()");
            return Ok();
        }
        [Route("paths")]
        public IActionResult Paths([FromBody] Dictionary<string, string> dic)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(dic));
            return Ok();
        }
    }
}
