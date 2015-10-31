using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;

namespace NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown
{
    internal class DaysLeftCountDownViewModel : ITileViewModel
    {
        private readonly IClock _clock;
        private readonly IDashboardController _dashboardController;
        public static readonly string TileTypeId = "TimeLeft.Days.Count";
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly TileConfiguration _tileConfiguration;
        private readonly DispatcherTimer _timer;
        private DaysLeftCountDownTileView _view;

        public DaysLeftCountDownViewModel(DashboardTileConfiguration tileConfiguration, IClock clock, IDashboardController dashboardController)
        {
            _clock = clock;
            _dashboardController = dashboardController;
            _tileConfiguration = new TileConfiguration(tileConfiguration, this);
            _timer = new DispatcherTimer {Interval = _tickPeriod};
            _timer.Tick += _timer_Tick;
        }

        public ICommand ConfigureCommand { get; private set; }

        public FrameworkElement CreateView()
        {
            ConfigureCommand = new TileConfigureCommand("Days Count Down Configuration", _tileConfiguration,
                new[]
                {
                    new ConfigurationParameter("Title", "TITLE", _tileConfiguration),
                    new ConfigurationParameter("End_date", new DateTime(2000, 1, 1), _tileConfiguration),
                    new ConfigurationParameter("Disabled", false, _tileConfiguration)
                },
                _dashboardController);

            _view = new DaysLeftCountDownTileView {DataContext = this};

            UpdateView();

            _timer.Start();
            return _view;
        }

        public void OnConfigurationChanged()
        {
            UpdateView();
            Tick();
        }

        private void UpdateView()
        {
            var disabled = _tileConfiguration.GetBool("Disabled");
            var endDate = _tileConfiguration.GetDateTime("End_date");
            var daysRemaining = (endDate - _clock.Now).Days;
            _view.days.Text = disabled ? "-" : daysRemaining.ToString();
            _view.header.Text = _tileConfiguration.GetString("Title");
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Tick();
        }

        private void Tick()
        {
            _timer.Stop();
            UpdateView();
            _timer.Start();
        }
    }
}