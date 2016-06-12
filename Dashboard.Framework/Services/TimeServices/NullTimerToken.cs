using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public sealed class NullTimerToken : ITimerToken
    {
        public DateTime DueDateTime { get; }
        public ITimerListener Listener { get; }
        public void Cancel()
        {
        }

        public void Requeue(TimeSpan timeSpan)
        {
        }
    }
}