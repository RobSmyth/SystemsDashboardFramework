using System;
using System.Windows;
using System.Windows.Input;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Time;

namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    internal class DateTileViewModel : ITileViewModel, ITimerListener
    {
        private readonly ITimerService _timerService;
        private readonly IClock _clock;
        public static readonly string TileTypeId = "Date.Now";
        private DateTileControl _view;

        public DateTileViewModel(ITimerService timerService, IClock clock)
        {
            _timerService = timerService;
            _clock = clock;
            ConfigureCommand = new NullCommand();
        }

        public FrameworkElement CreateView()
        {
            _view = new DateTileControl();
            UpdateView();
            _timerService.QueueCallback(TimeSpan.FromMilliseconds(100), this);
            return _view;
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged()
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