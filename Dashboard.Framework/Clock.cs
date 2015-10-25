using System;

namespace NoeticTools.Dashboard.Framework
{
    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
