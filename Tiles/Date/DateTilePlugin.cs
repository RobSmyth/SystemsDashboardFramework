using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Tiles.Date
{
    public sealed class DateTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.TileProviders.Register(new DateTileProvider(services));
        }
    }
}