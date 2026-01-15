
using ManagedCode.Storage.Server.Extensions.DependencyInjection;
using ManagedCode.Storage.VirtualFileSystem.Extensions;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Jellyfin.Plugin.FS;

public class PluginServiceRegistrator : IPluginServiceRegistrator
{
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddStorageServer();
        serviceCollection.AddStorageSignalR(); // optional
        serviceCollection.AddVirtualFileSystem(options =>
        {
            options.DefaultContainer = "vfs";
            options.EnableCache = false;
        });
    }
}
