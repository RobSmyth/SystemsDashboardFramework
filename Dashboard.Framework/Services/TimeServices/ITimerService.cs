using System;


namespace NoeticTools.SystemsDashboard.Framework.Services.TimeServices
{
    public interface ITimerService : IService
    {
        TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener);
        void FireAll();
    }
}