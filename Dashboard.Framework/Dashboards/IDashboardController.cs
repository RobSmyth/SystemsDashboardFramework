using System.Windows;
using NoeticTools.TeamStatusBoard.Framework.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Dashboards
{
    public interface IDashboardController
    {
        IDashboardNavigator DashboardNavigator { get; }
        IDashboardTileNavigator TileNavigator { get; }
        ITileDragAndDropController DragAndDropController { get; }
        void Start();
        void Stop();
        void ShowNavigationPane();
        void ShowOnSidePane(FrameworkElement viewController, string title);
        void Refresh();
        void ToggleGroupPanelsEditMode();
    }
}