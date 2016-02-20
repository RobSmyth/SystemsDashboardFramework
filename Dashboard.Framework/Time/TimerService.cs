using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;


namespace NoeticTools.SystemsDashboard.Framework.Time
{
    public sealed class TimerService : ITimerService, ITimerQueue, IService
    {
        private readonly IClock _clock;
        private readonly List<TimerToken> _callbacks = new List<TimerToken>();
        private readonly DispatcherTimer _timer;
        private readonly TimeSpan _tickRate = TimeSpan.FromMilliseconds(25);
        private bool _stopped;

        public TimerService(IClock clock)
        {
            _clock = clock;
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
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

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dueCallback = GetNextDueToken();
            while (dueCallback != null && stopwatch.Elapsed <= TimeSpan.FromSeconds(0.5) && !_stopped)
            {
                _callbacks.Remove(dueCallback);
                dueCallback.Listener.OnTimeElapsed(dueCallback);
                dueCallback = GetNextDueToken();
            }

            if (!_stopped)
            {
                _timer.Interval = _tickRate;
                _timer.Start();
            }
        }

        private TimerToken GetNextDueToken()
        {
            return _callbacks.FirstOrDefault(x => x != null && x.DueDateTime <= _clock.UtcNow);
        }

        public void Stop()
        {
            _timer.Stop();
            _stopped = true;
        }

        public void Start()
        {
            _stopped = false;
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();
        }
    }
}