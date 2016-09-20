using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.DataSources.TestData
{
    public class SawToothDatum : IDataValue, ITimerListener
    {
        private const double Minimum = 0.0;
        private const double Maximum = 10.0;
        private readonly TimeSpan _smallestTickPeriod = TimeSpan.FromMilliseconds(50);
        private readonly IDataValue _inner;
        private readonly TimeSpan _cyclePeriod;
        private readonly TimeSpan _tickPeriod;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly ITimerToken _timerToken;

        public SawToothDatum(IDataValue inner, TimeSpan cyclePeriod, IServices services)
        {
            _inner = inner;
            _cyclePeriod = cyclePeriod;
            _tickPeriod = TimeSpan.FromMilliseconds(Math.Max(_cyclePeriod.TotalMilliseconds/50, _smallestTickPeriod.TotalMilliseconds));
            _inner.Double = 0.0;
            _stopwatch.Start();
            _timerToken = services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public EventBroadcaster Broadcaster => _inner.Broadcaster;

        public string Name => _inner.Name;

        public PropertiesFlags Flags
        {
            get { return _inner.Flags; }
            set { _inner.Flags = value; }
        }

        public List<string> Tags => _inner.Tags;

        public object Instance
        {
            get { return _inner.Instance; }
            set { _inner.Instance = value; }
        }

        public double Double
        {
            get { return _inner.Double; }
            set { _inner.Double = value; }
        }

        public Color Colour
        {
            get { return _inner.Colour; }
            set { _inner.Colour = value; }
        }

        public string String
        {
            get { return _inner.String; }
            set { _inner.String = value; }
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            var elapsed = _stopwatch.Elapsed;
            if (elapsed.TotalSeconds > _cyclePeriod.TotalSeconds)
            {
                _stopwatch.Restart();
                elapsed = TimeSpan.Zero;
            }

            var cyclePosition = elapsed.TotalSeconds / _cyclePeriod.TotalSeconds;
            const double range = (Maximum - Minimum) * 2;
            if (cyclePosition < 0.5)
            {
                _inner.Double = (range * cyclePosition) + Minimum;
            }
            else
            {
                _inner.Double = Maximum - (range * (cyclePosition - 0.5));
            }

            _timerToken.Requeue(_tickPeriod);
        }
    }
}