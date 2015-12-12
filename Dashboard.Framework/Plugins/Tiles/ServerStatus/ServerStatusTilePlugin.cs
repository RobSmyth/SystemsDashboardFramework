using System;
using System.Windows;
using NoeticTools.SystemsDashboard.Framework.Config;
using NoeticTools.SystemsDashboard.Framework.Tiles.ServerStatus;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.ServerStatus
{
    internal sealed class ServerStatusTilePlugin : IPlugin, ITileControllerProvider
    {
        public int Rank => 0;

        public string Name => "Server status (future)";

        public bool MatchesId(string id)
        {
            return id == ServerStatusTileViewModel.TileTypeId || id.Equals("0FFACE9A-8B68-4DBC-8B42-0255F51368B4", StringComparison.InvariantCultureIgnoreCase);
        }

        public FrameworkElement CreateTile(TileConfiguration tile, TileLayoutController layoutController)
        {
            var view = new ServerStatusTileControl();
            new ServerStatusTileViewModel(tile, view);
            return view;
        }

        public TileConfiguration CreateDefaultConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Register(IServices services)
        {
            services.TileProviders.Register(this);
        }
    }
}