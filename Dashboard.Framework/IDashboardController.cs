using System.Windows.Controls;
using NoeticTools.Dashboard.Framework.Panes;

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
        void ShowOnSidePane(IPaneViewModel viewModel);
        void Refresh();
    }
}