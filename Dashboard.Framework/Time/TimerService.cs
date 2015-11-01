﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework
{
    public sealed class TimerService : ITimerService
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
            var callback = new TimerToken(listener, _clock);
            callback.Requeue(timeToCallback);
            _callbacks.Add(callback);
            return callback;
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

            var dueCallbacks = _callbacks.Where(x => x.DueDateTime <= _clock.UtcNow).ToArray();
            foreach (var dueCallback in dueCallbacks)
            {
                dueCallback.Listener.OnTimeElapsed(dueCallback);
            }

            _timer.Start();
        }
    }
}