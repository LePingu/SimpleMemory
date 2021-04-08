using System.Collections.Generic;
using System;
using System.Linq;
using NodaTime;
using System.Collections.Concurrent;

namespace SimpleMemory.CacheManager
{
    public class CacheCollectionManager<U, T>
    {
        // Extra protection, probably not needed
        ConcurrentDictionary<U, T> dictionaryCache = new ConcurrentDictionary<U, T>();

        // Wanted to use a Queue, but does not trace existing object in the middle.
        // Queue<T> queuedCache = new Queue<T>();
        // This seems inefficient in the long term, it would be better to record 
        // Data in a table just like your bitemporal data blog entry
        ConcurrentDictionary<U, LocalDateTime> keyTimestampDict = new ConcurrentDictionary<U, LocalDateTime>();
        List<(U, LocalDateTime)> recordEntries = new List<(U, LocalDateTime)>();
        private const int maxSize = 100;

        // Better implementation would be to monitor size of object with CLR API access
        // This is enough for a PoC
        // Each time a new object is assigned, but max number already present, Pop oldest object. 
        public T GetOrCreate(U key, T item)
        {
            var timeStamp = CreateLocalTimestamp();
            bool test = false;
            if (dictionaryCache.Count > 0)
            {
                test = dictionaryCache.First().Key.Equals(key);
            }
            if (dictionaryCache.ContainsKey(key))
            {
                keyTimestampDict[key] = timeStamp;
                recordEntries.Add((key, timeStamp));
                System.Console.WriteLine(this.dictionaryCache.Count);
                return dictionaryCache[key];
            }
            if (dictionaryCache.Count == maxSize)
            {
                var popKey = keyTimestampDict.OrderBy(x => x.Value).First().Key;
                dictionaryCache.TryRemove(popKey, out item);
                LocalDateTime removedStamp;
                keyTimestampDict.TryRemove(popKey, out removedStamp);
            }
            recordEntries.Add((key,timeStamp));
            dictionaryCache.TryAdd(key, item);
            keyTimestampDict.TryAdd(key, timeStamp);
            System.Console.WriteLine(this.dictionaryCache.Count);
            return item;
        }

        public void Flush()
        {
            dictionaryCache.Clear();
            keyTimestampDict.Clear();
            recordEntries.Clear();
        }

        public void FlushOld(LocalDateTime oldLimit)
        {
            var oldObjects = this.keyTimestampDict.Where(ts => ts.Value < oldLimit);
            foreach (var oldObject in oldObjects)
            {
                T oldValue;
                LocalDateTime oldTimeStamp;
                dictionaryCache.TryRemove(oldObject.Key, out oldValue);
                keyTimestampDict.TryRemove(oldObject.Key, out oldTimeStamp);
            }
        }

        public int GetSizeDictionary(){
            return this.dictionaryCache.Count;
        }

        public int GetSizeTimeStampDictionary(){
            return this.keyTimestampDict.Count;
        }

        public int GetRecordedEntries(){
            return this.recordEntries.Count;
        }

        public LocalDateTime GetTimeDict(U key){
            return this.keyTimestampDict[key];
        }

        private LocalDateTime CreateLocalTimestamp()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone tz = DateTimeZoneProviders.Tzdb["Europe/London"];
            LocalDateTime timeStamp = now.InZone(tz).LocalDateTime;
            return timeStamp;
        }
    }
}