using System;
using NodaTime;

namespace SimpleMemory.Helper
{    public class TimeHelper
    {
        public LocalDateTime CreateLocalTimestamp()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            DateTimeZone tz = DateTimeZoneProviders.Tzdb["Europe/London"];
            LocalDateTime timeStamp = now.InZone(tz).LocalDateTime;
            return timeStamp;
        }
    }
}