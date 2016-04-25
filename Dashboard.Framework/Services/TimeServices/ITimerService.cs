using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerService : IService
    {
        TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener);
        void FireAll();
    }
}