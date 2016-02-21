using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Tiles.ServerStatus;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class WmiTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromMinutes(1);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _machineName = string.Empty;
        private string _displayName = string.Empty;
        private readonly TimerToken _timerToken;
        private string _status = string.Empty;

        private readonly Dictionary<ServiceControllerStatus, Brush> _statusBrushes = new Dictionary<ServiceControllerStatus, Brush>
        {
            {ServiceControllerStatus.Running, Brushes.GreenYellow},
            {ServiceControllerStatus.PausePending, Brushes.CadetBlue},
            {ServiceControllerStatus.ContinuePending, (SolidColorBrush) (new BrushConverter().ConvertFrom("#FF448032"))},
            {ServiceControllerStatus.Paused, Brushes.Gray},
            {ServiceControllerStatus.StartPending, Brushes.Yellow},
            {ServiceControllerStatus.StopPending, Brushes.DarkSlateGray},
            {ServiceControllerStatus.Stopped, Brushes.Black},
        };

        private readonly Dictionary<ServiceControllerStatus, Brush> _statusTextBrushes = new Dictionary<ServiceControllerStatus, Brush>
        {
            {ServiceControllerStatus.Running, Brushes.White},
            {ServiceControllerStatus.PausePending, Brushes.White},
            {ServiceControllerStatus.ContinuePending, Brushes.White},
            {ServiceControllerStatus.Paused, Brushes.Firebrick},
            {ServiceControllerStatus.StartPending, Brushes.DarkSlateGray},
            {ServiceControllerStatus.StopPending, Brushes.White},
            {ServiceControllerStatus.Stopped, Brushes.White},
        };

        private Brush _ledBrush = Brushes.White;
        private string _value = "-";

        public WmiTileViewModel(TileConfiguration tile, ServerStatusTileControl view, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("Machine_Name", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Display_Name", "Text", _tileConfigurationConverter),
                new PropertyViewModel("WMI_Class", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Name_Property", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Name", "Text", _tileConfigurationConverter),
                new PropertyViewModel("Value_Property", "Text", _tileConfigurationConverter),
            };

            ConfigureCommand = new TileConfigureCommand(tile, "WMI Tile Configuration", parameters, dashboardController, layoutController, services);

            Update();
            view.DataContext = this;

            _timerToken = services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public string MachineName
        {
            get { return _machineName; }
            private set
            {
                if (!_machineName.Equals(value, StringComparison.InvariantCulture))
                {
                    _machineName = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush LedBrush
        {
            get { return _ledBrush; }
            private set
            {
                if (_ledBrush !=  value)
                {
                    _ledBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get { return _status; }
            private set
            {
                if (!_status.Equals(value, StringComparison.InvariantCulture))
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            private set
            {
                if (!_displayName.Equals(value, StringComparison.InvariantCulture))
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Value
        {
            get { return _value; }
            private set
            {
                if (!_value.Equals(value, StringComparison.InvariantCulture))
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ConfigureCommand { get; }

        private void Update()
        {
            MachineName = _tileConfigurationConverter.GetString("Machine_Name");
            DisplayName = _tileConfigurationConverter.GetString("Display_Name");
            var wmiClass = _tileConfigurationConverter.GetString("WMI_Class");
            var nameProperty = _tileConfigurationConverter.GetString("Name_Property");
            var name = _tileConfigurationConverter.GetString("Name");
            var valueProperty = _tileConfigurationConverter.GetString("Value_Property");

            if (!string.IsNullOrWhiteSpace(MachineName) && !string.IsNullOrWhiteSpace(DisplayName))
            {
                Task.Run(() =>
                {
                    var scope = new ManagementScope($@"\\{MachineName}\root\cimv2");

                    try
                    {
                        scope.Connect();
                        var query = new ObjectQuery($"SELECT * FROM {wmiClass}");
                        var searcher = new ManagementObjectSearcher(scope, query);
                        var managementObjectCollection = searcher.Get().Cast<ManagementObject>().ToArray();
                        var result = managementObjectCollection.FirstOrDefault(x => x.Properties.Cast<PropertyData>().Any(y => y.Name.Equals(nameProperty) && name.Equals(y.Value as string, StringComparison.InvariantCulture)));
                        return (string)result?[valueProperty];
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                })
                .ContinueWith(x =>
                {
                    if (x.Result != null)
                    {
                        Value = x.Result;
                        LedBrush = _statusBrushes[ServiceControllerStatus.Running];
                        Status = "OK";
                    }
                    else
                    {
                        Value = "-";
                        LedBrush = Brushes.DarkRed;
                    }
                    _timerToken.Requeue(_tickPeriod);
                });
            }
            else
            {
                Status = "Not configured";
                LedBrush = Brushes.Gray;
                _timerToken.Requeue(_tickPeriod);
            }
        }

        void IConfigurationChangeListener.OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
        }
    }
}