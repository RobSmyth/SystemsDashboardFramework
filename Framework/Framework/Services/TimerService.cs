using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Services
{
    public sealed class TimerService : ITimerService, ITimerQueue
    {
        private readonly TimeSpan _perTickTimeLimit = TimeSpan.FromSeconds(0.2);
        private readonly TimeSpan _tickRate = TimeSpan.FromMilliseconds(25);
        private readonly TimeSpan _firstTickDelay = TimeSpan.FromSeconds(0.1);
        private readonly IClock _clock;
        private readonly List<TimerToken> _callbacks = new List<TimerToken>();
        private readonly DispatcherTimer _timer;
        private bool _stopped;

        public TimerService(IClock clock)
        {
            _clock = clock;
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
        }

        public string Name => "Timer";

        public ITimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener)
        {
            var token = new TimerToken(listener, _clock, this);
            token.Requeue(timeToCallback);
            return token;
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
            _callbacks.Clear();
            foreach (var dueCallback in dueCallbacks)
            {
                dueCallback.Listener.OnTimeElapsed(dueCallback);
            }
        }

        public void Stop()
        {
            _timer.Stop();
            _stopped = true;
        }

        public void Start()
        {
            _stopped = false;
            _timer.Interval = _firstTickDelay;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dueCallback = GetNextDueToken();
            while (dueCallback != null && stopwatch.Elapsed <= _perTickTimeLimit && !_stopped)
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
            var callBacks = _callbacks.ToArray();
            return callBacks.FirstOrDefault(x => x != null && x.DueDateTime <= _clock.UtcNow);
        }
    }
}