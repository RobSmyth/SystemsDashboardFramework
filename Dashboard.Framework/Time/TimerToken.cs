using System;

namespace NoeticTools.Dashboard.Framework.Time
{
    public sealed class TimerToken
    {
        private readonly IClock _clock;

        internal TimerToken(ITimerListener listener, IClock clock)
        {
            _clock = clock;
            Listener = listener;
        }

        public DateTime DueDateTime { get; private set; }
        public ITimerListener Listener { get; private set; }

        public void Cancel()
        {
            Listener = new NullTimerListener();
            DueDateTime = DateTime.MinValue;
        }

        public void Requeue(TimeSpan timeSpan)
        {
            DueDateTime = _clock.UtcNow.Add(timeSpan);
        }
    }
}