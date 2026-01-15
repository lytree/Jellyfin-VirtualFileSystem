using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.FS.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public TabConfig[] Tabs { get; set; } = [];
    }

    public class TabConfig
    {
        public string ContentHtml { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
    }
}
