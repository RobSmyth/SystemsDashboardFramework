using System;


namespace NoeticTools.SystemsDashboard.Framework.Time
{
    public interface ITimerService
    {
        TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener);
        void FireAll();
    }
}