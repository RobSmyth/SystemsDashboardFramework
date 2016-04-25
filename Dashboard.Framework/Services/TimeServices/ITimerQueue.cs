namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerQueue
    {
        void Queue(TimerToken token);
    }
}