using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.Date
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

        public string Name => "Date";

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }

        public bool MatchesId(string id)
        {
            return id == DateTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B1", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tile, TileLayoutController layoutController)
        {
            return new DateTileController(_timerService, _clock);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = DateTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public int Rank => 0;
    }
}