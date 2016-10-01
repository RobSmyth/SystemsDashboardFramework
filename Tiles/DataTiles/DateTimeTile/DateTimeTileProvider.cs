using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Tiles.DataTiles.TextTile;


namespace NoeticTools.TeamStatusBoard.Tiles.DataTiles.DateTimeTile
{
    internal sealed class DateTimeDataTileProvider : ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;
        private static readonly string TileTypeId = "DateTimeData";

        public DateTimeDataTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Date & time property panel";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            return new TextDataTileControl {DataContext = new DateTimeDataTileViewModel(tileConfigturation, _dashboardController, layoutController, _services)};
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Tiles = new TileConfiguration[0]
            };
        }
    }
}