using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerService : IService
    {
        ITimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener);
        void FireAll();
    }
}