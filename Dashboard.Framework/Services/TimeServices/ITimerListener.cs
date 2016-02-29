namespace NoeticTools.SystemsDashboard.Framework.Services.TimeServices
{
    public interface ITimerListener
    {
        void OnTimeElapsed(TimerToken token);
    }
}