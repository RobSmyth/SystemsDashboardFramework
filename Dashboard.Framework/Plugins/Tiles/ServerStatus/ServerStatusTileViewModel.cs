using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Config.Commands;
using NoeticTools.SystemsDashboard.Framework.Config.Properties;
using NoeticTools.SystemsDashboard.Framework.Tiles.ServerStatus;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class ServerStatusTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _tickPeriod = TimeSpan.FromMinutes(1);
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _serverName = string.Empty;
        private string _serviceName = string.Empty;
        private TimerToken _timerToken;
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

        private Brush _ledBrush;

        public ServerStatusTileViewModel(TileConfiguration tile, ServerStatusTileControl view, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[]
            {
                new PropertyViewModel("ServerName", "Text", _tileConfigurationConverter),
                new PropertyViewModel("ServiceName", "Text", _tileConfigurationConverter),
            };

            ConfigureCommand = new TileConfigureCommand(tile, "Message Tile Configuration", parameters, dashboardController, layoutController, services);

            Update();
            view.DataContext = this;

            _timerToken = services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        public string ServerName
        {
            get { return _serverName; }
            private set
            {
                if (!_serverName.Equals(value, StringComparison.InvariantCulture))
                {
                    _serverName = value;
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

        public string ServiceName
        {
            get { return _serviceName; }
            private set
            {
                if (!_serviceName.Equals(value, StringComparison.InvariantCulture))
                {
                    _serviceName = value;
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

        public ICommand ConfigureCommand { get; }

        private void Update()
        {
            ServerName = _tileConfigurationConverter.GetString("ServerName");
            ServiceName = _tileConfigurationConverter.GetString("ServiceName");

            if (!string.IsNullOrWhiteSpace(ServerName) && !string.IsNullOrWhiteSpace(ServiceName))
            {
                Task.Run(() =>
                {
                    try
                    {
                        return ServiceController.GetServices(ServerName).SingleOrDefault(x => x.ServiceName.Equals(ServiceName, StringComparison.InvariantCulture));
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
                        Status = x.Result.Status.ToString();
                        LedBrush = _statusBrushes[x.Result.Status];
                    }
                    else
                    {
                        Status = "ERROR";
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