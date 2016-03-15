using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.BlankTile
{
    public class BlankTilePlugin : IPlugin
    {
        public const string TileTypeId = "Blank.Tile";

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new BlankTileProvider(services.DashboardController, services));
        }
    }
}