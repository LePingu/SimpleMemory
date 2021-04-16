using NodaTime;

namespace SimpleMemory.Models
{
    public class CacheEntry <U, T>
    {
        public T CachedObject { get; set; }
        public U LeftId { get; set; }
        public U RightId { get; set; }
        public U Id { get; set; }  
        public LocalDateTime TimeStamp { get; set; }
    }
    
}