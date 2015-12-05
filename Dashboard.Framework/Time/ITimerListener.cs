namespace NoeticTools.SystemsDashboard.Framework.Time
{
    public interface ITimerListener
    {
        void OnTimeElapsed(TimerToken token);
    }
}