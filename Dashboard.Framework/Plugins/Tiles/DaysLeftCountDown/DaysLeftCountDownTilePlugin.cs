﻿using System;
using NoeticTools.Dashboard.Framework.Config;
using NoeticTools.Dashboard.Framework.Tiles;
using NoeticTools.Dashboard.Framework.Tiles.DaysLeftCountDown;
using NoeticTools.Dashboard.Framework.Time;


namespace NoeticTools.Dashboard.Framework.Plugins.Tiles.DaysLeftCountDown
{
    public class DaysLeftCountDownTilePlugin : IPlugin, ITileControllerProvider
    {
        private readonly IDashboardController _dashboardController;
        private readonly IClock _clock;
        private readonly ITimerService _timerService;

        public DaysLeftCountDownTilePlugin(IDashboardController dashboardController, IClock clock, ITimerService timerService)
        {
            _dashboardController = dashboardController;
            _clock = clock;
            _timerService = timerService;
        }

        public string Name => "Days left";

        public bool MatchesId(string id)
        {
            return id == DaysLeftCountDownTileController.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B2", StringComparison.InvariantCultureIgnoreCase);
        }

        public IViewController CreateTileController(TileConfiguration tileConfiguration, TileLayoutController tileLayoutController)
        {
            return new DaysLeftCountDownTileController(tileConfiguration, _clock, _dashboardController, _timerService, tileLayoutController);
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = DaysLeftCountDownTileController.TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }

        public void Register(IServices services)
        {
            services.TileProviderRegistry.Register(this);
        }
    }
}