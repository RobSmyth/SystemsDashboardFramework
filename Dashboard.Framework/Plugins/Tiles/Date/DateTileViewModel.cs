using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Config.Commands;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date
{
    internal sealed class DateTileViewModel : ITimerListener
    {
        private readonly ITimerService _timerService;
        private readonly IClock _clock;
        private readonly DateTileControl _view;

        public DateTileViewModel(ITimerService timerService, IClock clock, DateTileControl view)
        {
            _timerService = timerService;
            _clock = clock;
            _view = view;
            ConfigureCommand = new NullCommand();
            _view.DataContext = this;
            _timerService.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public ICommand ConfigureCommand { get; }

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