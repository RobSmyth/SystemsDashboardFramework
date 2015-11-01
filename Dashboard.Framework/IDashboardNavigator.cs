namespace NoeticTools.Dashboard.Framework
{
    public interface IDashboardNavigator
    {
        void NextDashboard();
        void PrevDashboard();
        void ShowFirstDashboard();
        void ShowLastDashboard();
        void ShowCurrentDashboard();
        int CurrentDashboardIndex { get; }
        void ShowDashboard(int index);
        void ToggleShowPanesMode();
    }
}