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

        public string Name => "Timer";

        public TimerToken QueueCallback(TimeSpan timeToCallback, ITimerListener listener)
        {
            Console.WriteLine("== 3: QueueCallback");
            var token = new TimerToken(listener, _clock, this);
            token.Requeue(timeToCallback);
            return token;
        }

        public void Queue(TimerToken token)
        {
            Console.WriteLine("== 3: Queue");
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
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var dueCallback = GetNextDueToken();
            while (dueCallback != null && stopwatch.Elapsed <= TimeSpan.FromSeconds(0.5) && !_stopped)
            {
                Console.WriteLine("== 2a: {0}", _callbacks.Count);
                _callbacks.Remove(dueCallback);
                Console.WriteLine("== 2b: {0}", _callbacks.Count);
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
    }
}