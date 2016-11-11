using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.Tiles.ColouredTile
{
    public class ColouredTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly IServices _services;
        private IDataValue _background = new DataValue("", "Gray", PropertiesFlags.None);

        public ColouredTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ColouredTileControl view, ITileProperties properties)
            : base(properties)
        {
            _services = services;
            var parameters = GetConfigurationParameters();
            ConfigureCommand = new TileConfigureCommand(tile, "Coloured Tile Configuration", parameters, dashboardController, layoutController, services);
            Subscribe();
            view.DataContext = this;
        }

        public Brush Background => _background.SolidColourBrush;

        public ICommand ConfigureCommand { get; }

        private IEnumerable<IPropertyViewModel> GetConfigurationParameters()
        {
            var parameters = new IPropertyViewModel[]
            {
                new ColourPropertyViewModel("Background", Configuration, _services),
            };
            return parameters;
        }

        private void Subscribe()
        {
            _background = UpdateSubscription(_background, "Background", "Gray");
            OnPropertyChanged("Background");
        }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Subscribe();
        }
    }
}