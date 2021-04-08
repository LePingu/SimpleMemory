using Newtonsoft.Json;
using SimpleMemory.Models;
using Microsoft.Extensions.DependencyInjection;
using SimpleMemory.CacheManager;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace SimpleMemory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a service Collection for registration
            // We will not create the full implementation for DI here
            var serviceProvider = new ServiceCollection();

            var obj1 = new ArbitraryObject(1, "test");

            CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 2));
            CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 3));

            var builder = new HostBuilder().ConfigureServices(services => {

                // The following does not register properly
                // services.AddHostedService<BackgroundService>();

                // Other way to register a singleton directly through 
                // Dependency Injection
                services.AddSingleton<ISimpleCache, SimpleCache>();
            });
            await builder.RunConsoleAsync();
        }
    }
}
