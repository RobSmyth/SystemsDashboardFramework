using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;


namespace NoeticTools.Dashboard.Framework.Time
{
    public sealed class TimerService : ITimerService, ITimerQueue
    {
        private readonly IClock _clock;
        private readonly List<TimerToken> _callbacks = new List<TimerToken>();
        private readonly DispatcherTimer _timer;

        public TimerService(IClock clock)
        {
            _clock = clock;
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = TimeSpan.FromMilliseconds(25);
            _timer.Start();
        }

        public TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener)
        {
            var callback = new TimerToken(listener, _clock, this);
            callback.Requeue(timeToCallback);
            _callbacks.Add(callback);
            return callback;
        }

        public void Queue(TimerToken token)
        {
            if (!_callbacks.Contains(token))
            {
                _callbacks.Add(token);
            }
        }

        public void FireAll()
        {
            var dueCallbacks = _callbacks.ToArray();
            foreach (var dueCallback in dueCallbacks)
            {
                dueCallback.Listener.OnTimeElapsed(dueCallback);
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            var dueCallback = GetNextDueToken();
            while (dueCallback != null)
            {
                _callbacks.Remove(dueCallback);
                dueCallback.Listener.OnTimeElapsed(dueCallback);
                dueCallback = GetNextDueToken();
            }

            _timer.Start();
        }

        private TimerToken GetNextDueToken()
        {
            return _callbacks.FirstOrDefault(x => x.DueDateTime <= _clock.UtcNow);
        }
    }
}