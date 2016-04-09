using System;
using System.IO;
using System.Windows.Input;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles;
using NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ExpiredTimeAlert;
using NoeticTools.SystemsDashboard.Framework.Services.TimeServices;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.ExpiredTimeAlert
{
    /// <summary>
    /// Tile to show days since a file was touched. May also be configured to change colour on expired time thresholds.
    /// </summary>
    internal sealed class ExpiredTimeAlertTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITimerListener, ITileViewModel
    {
        private readonly IClock _clock;
        private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(30);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private readonly ExpiredTimeAlertTileView _view;
        private readonly TimerToken _timerToken;
        private string _daysSince;
        private string _title;
        private string _status = "Unknown";

        public ExpiredTimeAlertTileViewModel(TileConfiguration tile, IClock clock, IDashboardController dashboardController, ExpiredTimeAlertTileView view, TileLayoutController tileLayoutController,
            IServices services)
        {
            _clock = clock;
            _view = view;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            ConfigureCommand = new TileConfigureCommand(tile, "Expired Time Alert Configuration",
                new IPropertyViewModel[]
                {
                    new PropertyViewModel("Title", "Text", _tileConfigurationConverter),
                    new PropertyViewModel("File_path", "Text", _tileConfigurationConverter),
                    new PropertyViewModel("Warn_after_time", "TimeSpan", _tileConfigurationConverter),
                    new PropertyViewModel("Alert_after_time", "TimeSpan", _tileConfigurationConverter),
                    new PropertyViewModel("Disabled_Text", "Text", _tileConfigurationConverter),
                    new PropertyViewModel("Disabled", "Checkbox", _tileConfigurationConverter)
                },
            dashboardController, tileLayoutController, services);
            DaysSince = "";
            _view.DataContext = this;
            _timerToken = services.Timer.QueueCallback(TimeSpan.FromMilliseconds(10), this);
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

        public string Status
        {
            get { return _status; }
            set
            {
                if (string.IsNullOrWhiteSpace(_status) || !_status.Equals(value, StringComparison.InvariantCulture))
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DaysSince
        {
            get { return _daysSince; }
            private set
            {
                if (_daysSince != value)
                {
                    _daysSince = value;
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
            Title = _tileConfigurationConverter.GetString("Title");

            var disabled = _tileConfigurationConverter.GetBool("Disabled");
            if (disabled)
            {
                DaysSince = _tileConfigurationConverter.GetString("Disabled_Text");
                return;
            }

            var touchFilePath = _tileConfigurationConverter.GetString("File_path").Trim('"');
            if (File.Exists(touchFilePath))
            {
                var fileInfo = new FileInfo(touchFilePath);
                var expired = (_clock.UtcNow - fileInfo.LastWriteTimeUtc);
                DaysSince = ((int)expired.Days).ToString();

                if (SetExceeeded("ALERT", expired, "Alert_after_time")) return;
                if (SetExceeeded("NEAR ALERT", expired, "Warn_after_time")) return;

                var warnAfter = _tileConfigurationConverter.GetTimeSpan("Warn_after_time");
                
                Status = "RECENT";

                // TODO - SET COLOUR
            }
            else
            {
                DaysSince = "ERROR";
            }
        }

        private bool SetExceeeded(string threshold, TimeSpan expired, string parameter)
        {
            var alertAfter = _tileConfigurationConverter.GetTimeSpan(parameter);
            if (expired > alertAfter)
            {
                Status = threshold;
                return true;
            }
            return false;
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            UpdateView();
            _timerToken.Requeue(_tickPeriod);
        }
    }
}