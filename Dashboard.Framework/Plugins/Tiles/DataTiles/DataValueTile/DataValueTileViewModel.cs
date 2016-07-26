using System;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.DataTiles.DataValueTile
{
    internal sealed class DataValueTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public DataValueTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);

            var dataSourceTypeViewModel = new DataSourceTypePropertyViewModel("Datasource", _tileConfigurationConverter, _services);

            var parameters = GetConfigurationParameters(dataSourceTypeViewModel);
            ConfigureCommand = new TileConfigureCommand(tile, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
        }

        private IPropertyViewModel[] GetConfigurationParameters(INotifyingElementViewModel dataSourceTypeViewModel)
        {
            var parameters = new IPropertyViewModel[]
            {
                dataSourceTypeViewModel,
                new DependantPropertyViewModel("Property", "TextFromCombobox", _tileConfigurationConverter, dataSourceTypeViewModel,
                    () => _services.DataService.Get((string) dataSourceTypeViewModel.Value).GetAllNames().Cast<object>().ToArray())
            };
            return parameters;
        }

        public string Text
        {
            get { return _text; }
            private set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Update();
        }

        private void Update()
        {
            var datasource = _services.DataService.Get(_tileConfigurationConverter.GetString("Datasource"));
            Text = datasource.Read<string>(_tileConfigurationConverter.GetString("Property"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}