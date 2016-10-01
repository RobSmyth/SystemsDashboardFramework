using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Config.XmlTypes;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Styles;


namespace NoeticTools.TeamStatusBoard.Tiles.DaysLeftCountDown
{
    public sealed class DaysLeftCountDownTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "TimeLeft.Days.Count";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public DaysLeftCountDownTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Working days left to date count down";

        public string TypeId => TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B2", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var view = new DaysLeftCoundDownTileView();
            new StatusBoardStyleStategy(view, _services.Style);
            var conduit = new ConfigurationChangeListenerConduit();
            var tileProperties = new TileProperties(tileConfigturation, conduit, _services);
            var viewModel = new DaysLeftCountDownTileViewModel(tileConfigturation, _services.Clock, _dashboardController, view, layoutController, _services, tileProperties);
            conduit.SetTarget(viewModel);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            return new TileConfiguration
            {
                TypeId = TileTypeId,
                Id = Guid.NewGuid(),
                Tiles = new TileConfiguration[0]
            };
        }
    }
}