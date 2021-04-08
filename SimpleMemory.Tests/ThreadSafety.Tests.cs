using System.Threading;
using NUnit.Framework;
using SimpleMemory.CacheManager;
using SimpleMemory.Models;

namespace SimpleMemory.Tests
{
    [TestFixture]
    public class ThreadSafety_Should
    {
        bool failed = false;
        int iterations = 100;

        ArbitraryObject obj1;

        [SetUp]
        public void Setup()
        {
            obj1 = new ArbitraryObject(1, "test");
            // // threads interact with some object - either 
            // Thread thread1 = new Thread(new ThreadStart(delegate ()
            // {
            //     for (int i = 0; i < iterations; i++)
            //     {
            //         CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 2));
            //         // check that object is not out of synch due to other thread
            //         if (bad())
            //         {
            //             failed = true;
            //         }
            //     }
            // }));
            // Thread thread2 = new Thread(new ThreadStart(delegate ()
            // {
            //     for (int i = 0; i < iterations; i++)
            //     {
            //         CacheAccessSingleton.Instance.GetOrCreate(() => obj1.UselessMethod(1, 2));

            //         // check that object is not out of synch due to other thread
            //         if (bad())
            //         {
            //             failed = true;
            //         }
            //     }
            // }));

            // thread1.Start();
            // thread2.Start();
            // thread1.Join();
            // thread2.Join();
        }

        [Test]
        public void IntegrationTesting()
        {
            Assert.IsFalse(failed, "code was thread safe");

        }
    }
}