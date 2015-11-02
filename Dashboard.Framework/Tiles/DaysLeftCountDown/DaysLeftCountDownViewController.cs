using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NoeticTools.Dashboard.Framework.Commands;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Config.Commands;
using NoeticTools.Dashboard.Framework.Config.Parameters;


namespace NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown
{
    internal class DaysLeftCountDownViewController : IViewController
    {
        public const string TileTypeId = "TimeLeft.Days.Count";
        private readonly IClock _clock;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly DispatcherTimer _timer;
        private DaysLeftCountDownTileView _view;

        public DaysLeftCountDownViewController(TileConfiguration tileConfiguration, IClock clock,
            IDashboardController dashboardController)
        {
            _clock = clock;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
            _timer = new DispatcherTimer {Interval = _tickPeriod};
            _timer.Tick += _timer_Tick;
            ConfigureCommand = new TileConfigureCommand("Days Count Down Configuration", _tileConfigurationConverter,
                new[]
                {
                    new ConfigurationParameter("Title", "TITLE", _tileConfigurationConverter),
                    new ConfigurationParameter("End_date", new DateTime(2000, 1, 1), _tileConfigurationConverter),
                    new ConfigurationParameter("Disabled", false, _tileConfigurationConverter)
                },
                dashboardController);
        }

        public ICommand ConfigureCommand { get; }

        public FrameworkElement CreateView()
        {
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
            var disabled = _tileConfigurationConverter.GetBool("Disabled");
            var endDate = _tileConfigurationConverter.GetDateTime("End_date");
            var daysRemaining = (endDate - _clock.Now).Days;
            _view.days.Text = disabled ? "-" : daysRemaining.ToString();
            _view.header.Text = _tileConfigurationConverter.GetString("Title");
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