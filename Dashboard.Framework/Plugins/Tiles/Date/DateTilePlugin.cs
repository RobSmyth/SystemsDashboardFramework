using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Dashboards;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Date
{
    public sealed class DateTilePlugin : IPlugin, ITileControllerProvider
    {
        private const string TileTypeId = "Date.Now";
        private IServices _services;

        public int Rank => 0;

        public void Register(IServices services)
        {
            _services = services;
            services.TileProviders.Register(this);
        }

        public string Name => "Today's date";

        public bool MatchesId(string id)
        {
            return id == TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B1", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new DateTileControl();
            new DateTileViewModel(_services.Timer, _services.Clock, view);
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