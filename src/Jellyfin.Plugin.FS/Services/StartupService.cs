using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json.Nodes;
using Jellyfin.Plugin.FS.Helpers;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Playlists;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.FS.Services
{
    public class StartupService : IScheduledTask
    {
        public string Name => "FSPlugin Startup";

        public string Key => "Jellyfin.Plugin.FS.Startup";

        public string Description => "Startup Service for FSPlugin";

        public string Category => "Startup Services";

        private readonly ILogger<FSPlugin> _logger;

        public StartupService(ILogger<FSPlugin> logger)
        {
            _logger = logger;
        }

        public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"FSSystem Startup. Registering file transformations.");

            List<JsonNode> payloads = [];

            {
                JsonNode payload = new JsonObject
                {
                    { "id", "dcaafb64-88de-4efa-b77b-ae0616291cbb" },
                    { "fileNamePattern", "index.html" },
                    { "callbackAssembly", GetType().Assembly.FullName },
                    { "callbackClass", typeof(TransformationPatches).FullName },
                    { "callbackMethod", nameof(TransformationPatches.IndexHtml) }
                };

                payloads.Add(payload);
            }
            {
                JsonNode payload = new JsonObject
                {
                    { "id", "403e6374-7433-4137-b24f-2be01a14a90f" },
                    { "fileNamePattern", "home-html\\..*\\.chunk\\.js" },
                    { "callbackAssembly", GetType().Assembly.FullName },
                    { "callbackClass", typeof(TransformationPatches).FullName },
                    { "callbackMethod", nameof(TransformationPatches.HomeHtmlChunk) }
                };

                payloads.Add(payload);
            }

            Assembly? fileTransformationAssembly =
                AssemblyLoadContext.All.SelectMany(x => x.Assemblies).FirstOrDefault(x =>
                    x.FullName?.Contains(".FileTransformation") ?? false);

            if (fileTransformationAssembly != null)
            {
                Type? pluginInterfaceType = fileTransformationAssembly.GetType("Jellyfin.Plugin.FileTransformation.PluginInterface");

                if (pluginInterfaceType != null)
                {
                    foreach (JsonNode payload in payloads)
                    {
                        pluginInterfaceType.GetMethod("RegisterTransformation")?.Invoke(null, [payload]);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
            yield return new TaskTriggerInfo()
            {
                Type = TaskTriggerInfoType.StartupTrigger
            };
        }
    };
}

