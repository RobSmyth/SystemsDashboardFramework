using System;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public sealed class TimerToken
    {
        private readonly IClock _clock;
        private readonly ITimerQueue _queue;

        internal TimerToken(ITimerListener listener, IClock clock, ITimerQueue queue)
        {
            _clock = clock;
            _queue = queue;
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
            _queue.Queue(this);
        }
    }
}