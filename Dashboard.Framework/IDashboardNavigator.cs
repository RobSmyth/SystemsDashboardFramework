namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IDashboardNavigator
    {
        int CurrentDashboardIndex { get; }
        void NextDashboard();
        void PrevDashboard();
        void ShowFirstDashboard();
        void ShowLastDashboard();
        void ShowCurrentDashboard();
        void ShowDashboard(int index);
        void ToggleShowGroupPanelsDetailsMode();
    }
}