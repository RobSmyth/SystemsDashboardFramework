using NoeticTools.Dashboard.Framework.Tiles;


namespace NoeticTools.Dashboard.Framework
{
    public interface IDashboardController
    {
        void Start();
        void Stop();
        void ShowHelpPane();
        void ShowNavigationPane();
        void ShowOnSidePane(ITileViewModel viewModel);
        void Refresh();
    }
}