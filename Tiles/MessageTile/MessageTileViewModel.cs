using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Commands;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.Properties;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Tiles.MessageTile
{
    internal sealed class MessageTileViewModel : ConfiguredTileViewModelBase, IConfigurationChangeListener, ITileViewModel
    {
        private readonly TileConfigurationConverter _tileConfigurationConverter;
        private IDataValue _text = new NullDataValue();

        public MessageTileViewModel(TileConfiguration tile, IDashboardController dashboardController, ITileLayoutController layoutController, IServices services, TileProperties properties)
            : base(properties)
        {
            _tileConfigurationConverter = new TileConfigurationConverter(tile, this);
            var parameters = new IPropertyViewModel[] {new PropertyViewModel("Message", PropertyType.Text, _tileConfigurationConverter)};
            ConfigureCommand = new TileConfigureCommand(tile, "Message Tile Configuration", parameters, dashboardController, layoutController, services);
            Subscribe();
        }

        private void Subscribe()
        {
            _text = UpdateSubscription(_text, "Label", "Label");
        }

        public string Text => _text.String;

        public ICommand ConfigureCommand { get; }

        public void OnConfigurationChanged(TileConfigurationConverter converter)
        {
            Subscribe();
        }
    }
}