using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class WmiTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "Server.Status";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public WmiTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public int Rank => 0;

        public string Name => "Server status";

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new ServerStatusTileControl();
            new WmiTileViewModel(tile, view, _dashboardController, layoutController, _services);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}