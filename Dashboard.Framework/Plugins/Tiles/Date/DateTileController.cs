using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Commands;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date
{
    internal sealed class DateTileController : IViewController, ITimerListener
    {
        public const string TileTypeId = "Date.Now";
        private readonly ITimerService _timerService;
        private readonly IClock _clock;
        private DateTileControl _view;

        public DateTileController(ITimerService timerService, IClock clock)
        {
            _timerService = timerService;
            _clock = clock;
            ConfigureCommand = new NullCommand();
        }

        public ICommand ConfigureCommand { get; }

        public FrameworkElement CreateView()
        {
            _view = new DateTileControl();
            UpdateView();
            _timerService.QueueCallback(TimeSpan.FromMilliseconds(100), this);
            return _view;
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
        }

        private void UpdateView()
        {
            var now = _clock.Now;
            _view.day.Text = now.Day.ToString();
            _view.month.Text = now.ToString("MMM");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            UpdateView();
            var now = _clock.Now;
            var timeToNextMinuteChange = TimeSpan.FromSeconds(60.1 - now.Second);
            _timerService.QueueCallback(timeToNextMinuteChange, this);
        }
    }
}