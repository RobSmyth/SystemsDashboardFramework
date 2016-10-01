using System;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.TimeServices;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.DataValueTile
{
    internal sealed class DataValueTileViewModel : NotifyingViewModelBase, IConfigurationChangeListener, ITileViewModel, ITimerListener
    {
        private readonly TimeSpan _updatePeriod = TimeSpan.FromSeconds(30);
        private readonly IServices _services;
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private string _text;

        public DataValueTileViewModel(TileConfiguration tileConfiguration, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services)
        {
            _services = services;
            _tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);

            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tileConfiguration, "Data Value Tile Configuration", parameters, dashboardController, layoutController, services);

            _services.Timer.QueueCallback(TimeSpan.FromMilliseconds(100), this);
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

        private IPropertyViewModel[] GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new TextPropertyViewModel("Value", _tileConfigurationConverter, _services)
            };
            return parameters;
        }

        private void Update()
        {
            // todo
            //Text = datasource.Read<string>(_tileConfigurationConverter.GetString("Property"));
        }

        void ITimerListener.OnTimeElapsed(TimerToken token)
        {
            Update();
            _services.Timer.QueueCallback(_updatePeriod, this);
        }
    }
}