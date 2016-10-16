using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.TextTile
{
    internal sealed class TextDataTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private IDataValue _text = new DataValue("", "Label", PropertiesFlags.None);

        public TextDataTileViewModel(TileConfiguration tileConfiguration, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, ITileProperties properties)
            : base(properties)
        {
            var tileConfigurationConverter = new TileConfigurationConverter(tileConfiguration, this);
            var parameters = new IPropertyViewModel[] {new TextPropertyViewModel("PropertyAddress", tileConfigurationConverter, services)};
            ConfigureCommand = new TileConfigureCommand(tileConfiguration, "Text Data Tile Configuration", parameters, dashboardController, layoutController, services);
            Subscribe();
        }

        public string Text => _text.String;

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(INamedValueRepository converter)
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _text = UpdateSubscription(_text, "Text", "Text");
            OnPropertyChanged("Text");
        }
    }
}