using System;


namespace NoeticTools.Dashboard.Framework.Time
{
    public interface ITimerService
    {
        TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener);
        void FireAll();
    }
}