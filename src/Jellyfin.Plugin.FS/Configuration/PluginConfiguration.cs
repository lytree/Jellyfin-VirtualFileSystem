using ManagedCode.Storage.Core;
using ManagedCode.Storage.VirtualFileSystem.Options;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.FS.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PathMappings[] FileSystems { get; set; } = [];
    }

    public class PathMappings
    {
        public string LinkPath { get; set; } = string.Empty;
        public string BaseFolder { get; set; } = string.Empty;
        //public Dictionary<string, string> Options { get; set; } = new();

    }
}
