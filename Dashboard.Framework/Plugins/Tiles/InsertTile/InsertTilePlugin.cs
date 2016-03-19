using NoeticTools.SystemsDashboard.Framework.Input;
using NoeticTools.SystemsDashboard.Framework.Services;


namespace NoeticTools.SystemsDashboard.Framework.Plugins.Tiles.InsertTile
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