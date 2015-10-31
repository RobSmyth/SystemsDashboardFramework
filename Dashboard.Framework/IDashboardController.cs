using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Tiles;

namespace NoeticTools.Dashboard.Framework
{
    public interface IDashboardController
    {
        void Start(Grid tileGrid, DockPanel sidePanel);
        void Stop();
        void NextDashboard();
        void PrevDashboard();
        void ShowFirstDashboard();
        void ShowLastDashboard();
        void ShowCurrentDashboard();
        void ShowHelp();
        void ShowNavigation();
        void ShowOnSidePane(ITileViewModel viewModel);
        void Refresh();
        void ShowDashboard(int index);
        int DashboardIndex { get; }
    }
}