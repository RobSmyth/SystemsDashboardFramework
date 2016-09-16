using System;
using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Config;
using NoeticTools.TeamStatusBoard.Framework.Dashboards;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.Image
{
    internal sealed class ImageFileWatcherTileProvider : ITileControllerProvider
    {
        private const string TileTypeId = "Image.File.Watcher";
        private readonly IDashboardController _dashboardController;
        private readonly IServices _services;

        public ImageFileWatcherTileProvider(IDashboardController dashboardController, IServices services)
        {
            _dashboardController = dashboardController;
            _services = services;
        }

        public string Name => "Image file watcher";

        public string TypeId => ImageFileWatcherTileProvider.TileTypeId;

        public bool MatchesId(string id)
        {
            return id == TileTypeId;
        }

        public FrameworkElement CreateTile(TileConfiguration tileConfigturation, TileLayoutController layoutController)
        {
            var view = new ImageFileWatcherTileControl();
            new ImageFileWatcherViewModel(tileConfigturation, _dashboardController, layoutController, _services, view);
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