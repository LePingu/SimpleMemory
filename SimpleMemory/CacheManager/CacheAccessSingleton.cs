using System;
using System.Linq.Expressions;
using NodaTime;

namespace SimpleMemory.CacheManager
{
    public sealed class CacheAccessSingleton
    {
        CacheAccessSingleton()
        {
        }

        public static CacheAccessSingleton Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        Lazy<CacheCollectionManager<string, object>> cacheCollectionManager = new Lazy<CacheCollectionManager<string, object>>();

        // We need the expression to get access to the reflection info of the incoming method
        // This way we can create a unique key for any method coming in, coupled with it's parameters
        public object GetOrCreate<T>(Expression<Func<T>> createItem)
        {
            var methodCallExpression = createItem.Body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                // Create the key
                var key = CacheHelper.CreateKey(methodCallExpression);

                var result = cacheCollectionManager.Value.Get(key);
                if (result is null)
                {
                    // Compile the lambda expression.  
                    Func<T> compiledExpression = createItem.Compile();
                    // Execute the lambda expression to create thev value.  
                    var heavyMethodResult = compiledExpression();
                    result = cacheCollectionManager.Value.Create(key, heavyMethodResult);
                }
                return result;
            }
            return null;
        }

        public void FlushOld(LocalDateTime oldLimit)
        {
            cacheCollectionManager.Value.FlushOld(oldLimit);
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly CacheAccessSingleton instance = new CacheAccessSingleton();
        }
    }
}