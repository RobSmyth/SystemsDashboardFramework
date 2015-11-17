using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles.Date;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;


namespace NoeticTools.Dashboard.Framework.Tiles.WebPage
{
    public class WebPageTilePlugin : IPlugin, IViewControllerProvider
    {
        private readonly IDashboardController _dashboardController;

        public WebPageTilePlugin(IDashboardController dashboardController)
        {
            _dashboardController = dashboardController;
        }

        public bool MatchesId(string id)
        {
            return id == WebPageTileController.TileTypeId || id.Equals("92CE0D61-4748-4427-8EB7-DC8B8B741C15", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateViewController(TileConfiguration tileConfiguration)
        {
            return new WebPageTileController(tileConfiguration, _dashboardController);
        }

        public void Register(IServices services)
        {
            services.Repository.Register(this);
        }
    }
}