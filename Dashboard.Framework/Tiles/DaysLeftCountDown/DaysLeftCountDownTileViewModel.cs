using System;
using System.Windows.Input;
using System.Windows.Threading;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Parameters;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown
{
    internal class DaysLeftCountDownTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITimerListener
    {
        private readonly IClock _clock;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly DaysLeftCountDownTileView _view;
        private readonly TimerToken _timerToken;

        public DaysLeftCountDownTileViewModel(TileConfiguration tileConfiguration, IClock clock, IDashboardController dashboardController, DaysLeftCountDownTileView view, ITimerService timerService)
        {
            _clock = clock;
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
            ConfigureCommand = new TileConfigureCommand("Days Count Down Configuration",
                new IElementViewModel[]
                {
                    new ElementViewModel("Title", ElementType.Text, _tileConfigurationConverter),
                    new ElementViewModel("End_date", ElementType.DateTime, _tileConfigurationConverter),
                    new ElementViewModel("Disabled", ElementType.Boolean, _tileConfigurationConverter)
                },
                dashboardController);
            _view.DataContext = this;
            _timerToken = timerService.QueueCallback(TimeSpan.FromMilliseconds(10), this);
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var disabled = _tileConfigurationConverter.GetBool("Disabled");
            var endDate = _tileConfigurationConverter.GetDateTime("End_date");
            var now = _clock.Now;
            var daysRemaining = Math.Abs((endDate - now).Days);
            var weeksRemaining = daysRemaining/7;
            var workingDaysRemaining = (weeksRemaining*5) + daysRemaining%7;
            if (now > endDate)
            {
                workingDaysRemaining = -workingDaysRemaining;
            }
            _view.days.Text = disabled ? "-" : workingDaysRemaining.ToString();
            _view.header.Text = _tileConfigurationConverter.GetString("Title");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            UpdateView();
            _timerToken.Requeue(_tickPeriod);
        }
    }
}