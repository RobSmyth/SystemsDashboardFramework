using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.HelpTile
{
    public sealed class HelpTilePlugin : IPlugin
    {
        public int Rank => 0;

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(new HelpTileKeyHandler(services.DashboardController));
        }
    }
}