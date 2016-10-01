using System;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.SuggestionProviders;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.DaysLeftCountDown
{
    internal sealed class DaysLeftCountDownTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITimerListener, ITileViewModel
    {
        private readonly IClock _clock;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly ITimerToken _timerToken;
        private string _daysRemaining;
        private string _title;
        private IDataValue _disabled = new NullDataValue();

        public DaysLeftCountDownTileViewModel(TileConfiguration tile, IClock clock, IDashboardController dashboardController, DaysLeftCoundDownTileView view, ITileLayoutController tileLayoutController, IServices services, TileProperties properties)
            : base(properties)
        {
            _clock = clock;
            ConfigureCommand = new TileConfigureCommand(tile, "Days Count Down Configuration",
                new IPropertyViewModel[]
                {
                    new TextPropertyViewModel("Title", Configuration, services),
                    new PropertyViewModel("End_date", PropertyType.DateTime, Configuration, new NullSuggestionProvider()),
                    new TextPropertyViewModel("Disabled", Configuration, services)
                },
                dashboardController, tileLayoutController, services);
            DaysRemaining = "";
            view.DataContext = this;

            Subscribe();

            _timerToken = services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
        }

        private void Subscribe()
        {
            _disabled = UpdateSubscription(_disabled, "Disabled", "False");
        }

        public ICommand ConfigureCommand { get; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(_title) || !_title.Equals(value, StringComparison.InvariantCulture))
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DaysRemaining
        {
            get { return _daysRemaining; }
            private set
            {
                if (_daysRemaining != value)
                {
                    _daysRemaining = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            var endDate = Configuration.GetDateTime("End_date");
            var now = _clock.Now;
            var daysRemaining = Math.Abs((endDate - now).Days);
            var weeksRemaining = daysRemaining/7;
            var workingDaysRemaining = (weeksRemaining*5) + daysRemaining%7;
            if (now > endDate)
            {
                workingDaysRemaining = -workingDaysRemaining;
            }
            DaysRemaining = _disabled.Boolean ? "-" : workingDaysRemaining.ToString();
            Title = Configuration.GetString("Title");
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            UpdateView();
            _timerToken.Requeue(_tickPeriod);
        }
    }
}