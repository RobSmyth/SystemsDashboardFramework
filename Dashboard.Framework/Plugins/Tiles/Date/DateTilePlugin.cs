using System;
using NoeticTools.SystemsDashboard.Framework.Commands;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Time;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date
{
    public sealed class DateTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly ITimerService _timerService;
        private readonly IClock _clock;

        public DateTilePlugin(ITimerService timerService, IClock clock)
        {
            _timerService = timerService;
            _clock = clock;
        }

        public string Name => "Date";

        public int Rank => 0;

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
    }
}