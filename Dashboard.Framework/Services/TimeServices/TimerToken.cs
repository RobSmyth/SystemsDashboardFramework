using System;
using System.Security.Cryptography.X509Certificates;


namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public sealed class TimerToken : ITimerToken
    {
        private readonly IClock _clock;
        private readonly ITimerQueue _queue;
        private Action<TimeSpan> _requeueAction;

        internal TimerToken(ITimerListener listener, IClock clock, ITimerQueue queue)
        {
            _clock = clock;
            _queue = queue;
            Listener = listener;
            _requeueAction = x =>
            {
                DueDateTime = _clock.UtcNow.Add(x);
                _queue.Queue(this);
            };
        }

        public DateTime DueDateTime { get; private set; }
        public ITimerListener Listener { get; private set; }

        public void Cancel()
        {
            _requeueAction = x => { };
            Listener = new NullTimerListener();
            DueDateTime = DateTime.MinValue;
        }

        public void Requeue(TimeSpan timeSpan)
        {
            _requeueAction(timeSpan);
        }
    }
}