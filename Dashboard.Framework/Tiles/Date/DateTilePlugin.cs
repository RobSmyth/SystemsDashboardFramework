using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Tiles.Date
{
    public class DateTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly ITimerService _timerService;
        private readonly IClock _clock;

        public DateTilePlugin(ITimerService timerService, IClock clock)
        {
            _timerService = timerService;
            _clock = clock;
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }

        public bool MatchesId(string id)
        {
            return id == DateTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B1", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration)
        {
            return new DateTileController(_timerService, _clock);
        }

        public string Name => "Date";
    }
}