namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.Help
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