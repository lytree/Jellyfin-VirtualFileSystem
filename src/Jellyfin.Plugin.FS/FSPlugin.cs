using Jellyfin.Plugin.FS.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.FS;
/// <summary>
/// 虚拟文件系统
/// </summary>
public class FSPlugin : BasePlugin<PluginConfiguration>, IHasPluginConfiguration, IHasWebPages
{
    public override Guid Id => Guid.Parse("235c7b78-2fa6-4a7a-8ffb-7929d5ee3e56");
    public override string Name => "FSSystem";

    public static FSPlugin Instance { get; private set; } = null!;

    public FSPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        string? prefix = GetType().Namespace;

        yield return new PluginPageInfo
        {
            Name = Name,
            EmbeddedResourcePath = $"{prefix}.Configuration.config.html"
        };
    }
}
