using System.Collections.Generic;
using System;
using System.Linq;
using NodaTime;
using System.Collections.Concurrent;
using SimpleMemory.Models;
using SimpleMemory.Helper;

namespace SimpleMemory.CacheManager
{
    public class CacheCollectionManager<U, T>
    {
        // Extra protection, probably not needed
        ConcurrentDictionary<U, CacheEntry<U, T>> dictionaryCache;
        Lazy<List<(U, LocalDateTime)>> recordEntries;
        TimeHelper timeHelper;
        CacheEntry<U, T> topItem;
        CacheEntry<U, T> bottomItem;
        private const int maxSize = 100;

        public CacheCollectionManager()
        {
            this.dictionaryCache = new ConcurrentDictionary<U, CacheEntry<U, T>>();
            this.recordEntries = new Lazy<List<(U, LocalDateTime)>>();
            this.timeHelper = new TimeHelper();
            topItem = new CacheEntry<U, T>();
            bottomItem = new CacheEntry<U, T>();
        }

        // Better implementation would be to monitor size of object with CLR API access
        // This is enough for a PoC
        // Each time a new object is assigned, but max number already present, Pop oldest object. 
        public T Get(U key)
        {
            var timeStamp = this.timeHelper.CreateLocalTimestamp();
            recordEntries.Value.Add((key, timeStamp));

            if (dictionaryCache.ContainsKey(key))
            {
                dictionaryCache[key].TimeStamp = timeStamp;
                return (T)dictionaryCache[key].CachedObject;
            }
            return default;
        }

        public T Create(U key, T item)
        {
            var timeStamp = this.timeHelper.CreateLocalTimestamp();
            recordEntries.Value.Add((key, timeStamp));

            if (dictionaryCache.Count == maxSize)
            {
                bottomItem = dictionaryCache[bottomItem.LeftId];
                var removedItem = dictionaryCache[bottomItem.Id];
                var truc = dictionaryCache.Remove(bottomItem.RightId, out removedItem);
                bottomItem.RightId = default(U);
            }
            var previousTopId = topItem.Id;
            topItem.LeftId = key;
            topItem = new CacheEntry<U, T>
            {
                CachedObject = item,
                Id = key,
                RightId = previousTopId,
                TimeStamp = timeStamp
            };
            dictionaryCache.TryAdd(key, topItem);
            if (dictionaryCache.Count == 2)
            {
                bottomItem = dictionaryCache[topItem.RightId];
            }
            return item;
        }

        public void Flush()
        {
            dictionaryCache.Clear();
            recordEntries.Value.Clear();
        }

        public void FlushOld(LocalDateTime oldLimit)
        {
            var oldObjects = this.dictionaryCache.Where(ts => ts.Value.TimeStamp < oldLimit);
            foreach (var oldObject in oldObjects)
            {
                CacheEntry<U, T> oldValue;
                dictionaryCache.TryRemove(oldObject.Key, out oldValue);
            }
        }

        public int GetSizeDictionary()
        {
            return this.dictionaryCache.Count;
        }

        public int GetRecordedEntries()
        {
            return this.recordEntries.Value.Count;
        }
    }
}