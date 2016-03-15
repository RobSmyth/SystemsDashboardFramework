using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;
        private IServices _services;
        public const string TileTypeId = "Blank.Tile";

        public BlankTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(new BlankTileProvider(_dashboardController, _services));
        }
    }
}