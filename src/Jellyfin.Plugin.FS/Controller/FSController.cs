using Jellyfin.Plugin.FS.Configuration;
using ManagedCode.Storage.Core;
using ManagedCode.Storage.Server.ChunkUpload;
using ManagedCode.Storage.Server.Controllers;
using ManagedCode.Storage.VirtualFileSystem.Core;
using Microsoft.AspNetCore.Mvc;

namespace Jellyfin.Plugin.FS.Controller
{
    [ApiController]
    [Route("fs/api/storage")]
    public class FSController : StorageControllerBase<IStorage>
    {
        /// <summary>
        /// Initialises a new instance of the default storage controller.
        /// </summary>
        /// <param name="storage">The shared storage instance.</param>
        /// <param name="chunkUploadService">Chunk upload coordinator.</param>
        /// <param name="options">Server behaviour options.</param>
        public FSController(
            IStorage storage,
            ChunkUploadService chunkUploadService,
            StorageServerOptions options) : base(storage, chunkUploadService, options)
        {
        }
    }
}
