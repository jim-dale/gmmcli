using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace gmmcli
{
    internal static class ServiceCollectionExtensions
    {
        public static ServiceCollection ConfigureServices(this ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddTransient<GrooveMusicService>()
                .AddTransient<AudioFilesService>()
                .AddTransient<Worker>();

            return services;
        }
    }
}
