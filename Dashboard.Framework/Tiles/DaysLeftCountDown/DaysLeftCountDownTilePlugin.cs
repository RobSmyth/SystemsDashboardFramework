using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;


namespace NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown
{
    public class DaysLeftCountDownTilePlugin : ITilePlugin
    {
        private readonly IDashboardController _dashboardController;
        private readonly IClock _clock;

        public DaysLeftCountDownTilePlugin(IDashboardController dashboardController, IClock clock)
        {
            _dashboardController = dashboardController;
            _clock = clock;
        }

        public bool MatchesId(string id)
        {
            return id == DaysLeftCountDownTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B2", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateViewController(TileConfiguration tileConfiguration)
        {
            return new DaysLeftCountDownTileController(tileConfiguration, _clock, _dashboardController);
        }
    }
}