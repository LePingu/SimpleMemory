using System.Threading;
using NodaTime;
using NUnit.Framework;
using SimpleMemory.CacheManager;
using SimpleMemory.Models;

namespace SimpleMemory.Tests
{
    [TestFixture]
    public class CacheCollectionManager_Should
    {
        CacheCollectionManager<int, string> cacheCollectionManager;
        [SetUp]
        public void Setup()
        {
            cacheCollectionManager = new CacheCollectionManager<int, string>();
            for (int i = 0; i < 100; i++)
            {
                cacheCollectionManager.GetOrCreate(i, "test"+i);
            }
        }

        [Test]
        public void CacheDict_AlwaysStay_100_When_New_Item()
        {
            cacheCollectionManager.GetOrCreate(102, "other");
            Assert.AreEqual(cacheCollectionManager.GetSizeDictionary(), 100);
        }

        [Test]
        public void TimeStampDict_AlwaysStay_100_When_New_Item()
        {
            cacheCollectionManager.GetOrCreate(102, "other");
            Assert.AreEqual(cacheCollectionManager.GetSizeTimeStampDictionary(), 100);
        }

        [Test]
        public void RecordedEntries_Contains_Accurate_Number_Entries()
        {
            cacheCollectionManager.GetOrCreate(102, "other");
            Assert.AreEqual(cacheCollectionManager.GetRecordedEntries(), 101);
        }

        [Test]
        public void Dict_Cannot_Have_Double_Entries_And_hasTimeStamp()
        {
            cacheCollectionManager.GetOrCreate(102, "other");
            cacheCollectionManager.GetOrCreate(102, "else");
            Assert.IsNotNull(cacheCollectionManager.GetTimeDict(102));
            Assert.That(cacheCollectionManager.GetTimeDict(102), Is.TypeOf(typeof(LocalDateTime)));
        }
    }
}