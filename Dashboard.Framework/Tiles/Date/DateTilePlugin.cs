using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles.Message;
using NoeticTools.Dashboard.Framework.Tiles.TeamCityLastBuildStatus;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public class DateTilePlugin : ITilePlugin
    {
        private readonly ITimerService _timerService;
        private readonly IDashboardController _dashboardController;
        private readonly IClock _clock;

        public DateTilePlugin(ITimerService timerService, IDashboardController dashboardController, IClock clock)
        {
            _timerService = timerService;
            _dashboardController = dashboardController;
            _clock = clock;
        }

        public bool MatchesId(string id)
        {
            return id == DateTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B1", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateViewController(TileConfiguration tileConfiguration)
        {
            return new DateTileController(_timerService, _clock);
        }
    }
}