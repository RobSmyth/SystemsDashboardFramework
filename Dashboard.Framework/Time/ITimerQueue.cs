namespace NoeticTools.SystemsDashboard.Framework.Time
{
    public interface ITimerQueue
    {
        void Queue(TimerToken token);
    }
}