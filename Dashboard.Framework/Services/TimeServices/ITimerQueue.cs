namespace NoeticTools.SystemsDashboard.Framework.Services.TimeServices
{
    public interface ITimerQueue
    {
        void Queue(TimerToken token);
    }
}