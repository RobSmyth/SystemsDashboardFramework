using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.MessageTile
{
    internal sealed class MessageTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private IServices _services;
        public static readonly string TileTypeId = "Message";

        public MessageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public string Name => "Message";

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B3", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            return new MessageTileControl { DataContext = new MessageViewModel(tile, _dashboardController, layoutController, _services) };
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

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(this);
        }
    }
}