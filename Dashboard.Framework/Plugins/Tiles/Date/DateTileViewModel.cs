using System;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework.Config.Commands;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date
{
    internal sealed class DateTileViewModel : NotifyingViewModelBase, ITimerListener, ITileViewModel
    {
        private readonly ITimerService _timerService;
        private readonly IClock _clock;
        private readonly DateTileControl _view;
        private int _day;
        private string _month;

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

        public int Day
        {
            get { return _day; }
            private set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Month
        {
            get { return _month; }
            set
            {
                if (_month != value)
                {
                    _month = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateView()
        {
            var now = _clock.Now;
            Day = now.Day;
            Month = now.ToString("MMM");
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