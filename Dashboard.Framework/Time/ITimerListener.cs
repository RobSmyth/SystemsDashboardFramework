namespace NoeticTools.Dashboard.Framework.Time
{
    public interface ITimerListener
    {
        void OnTimeElapsed(TimerToken token);
    }
}