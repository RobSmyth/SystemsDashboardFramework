using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ExpiredTimeAlert
{
    public class ExpiredTimeAlertTilePlugin : IPlugin
    {
        private readonly IDashboardController _dashboardController;

        public ExpiredTimeAlertTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new ExpiredTimeAlertTileProvider(_dashboardController, services));
        }
    }
}