using Microsoft.Extensions.DependencyInjection;

namespace gmmcli
{
    class Program
    {
        static int Main(string[] args)
        {
            using var serviceProvider = new ServiceCollection()
                .ConfigureServices()
                .BuildServiceProvider();

            return serviceProvider.GetService<Worker>()
                .Run(args);
        }
    }
}
