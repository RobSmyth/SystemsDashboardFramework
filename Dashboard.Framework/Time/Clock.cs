using System;


namespace NoeticTools.SystemsDashboard.Framework.Time
{
    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}