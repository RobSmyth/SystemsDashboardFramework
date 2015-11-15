namespace NoeticTools.Dashboard.Framework.Time
{
    public interface ITimerQueue
    {
        void Queue(TimerToken token);
    }
}