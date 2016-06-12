using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerToken
    {
        DateTime DueDateTime { get; }
        ITimerListener Listener { get; }
        void Cancel();
        void Requeue(TimeSpan timeSpan);
    }
}