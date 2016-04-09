using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.Tiles.InsertTile
{
    internal sealed class InsertTilePlugin : IPlugin
    {
        public int Rank => 25;

        public void Register(IServices services)
        {
            services.KeyboardHandler.Register(new InsertTileKeyHandler(services.DashboardController, services.DashboardController.DragAndDropController, services));
        }
    }
}