using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerToken
    {
        void Cancel();
        void Requeue(TimeSpan timeSpan);
    }
}