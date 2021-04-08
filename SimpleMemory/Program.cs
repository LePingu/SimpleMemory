using Newtonsoft.Json;
using SimpleMemory.Models;
using Microsoft.Extensions.DependencyInjection;
using SimpleMemory.CacheManager;

namespace SimpleMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a service Collection for registration
            // We will not create the full implementation for DI here
            var serviceProvider = new ServiceCollection();

            var obj1 = new ArbitraryObject(1, "test");
            
            CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 2));
            CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 2));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Other way to register a singleton directl through 
            // Dependency Injection
            services.AddSingleton<ISimpleCache, SimpleCache>();
        }
    }
}
