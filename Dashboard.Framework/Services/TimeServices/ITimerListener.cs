namespace NoeticTools.TeamStatusBoard.Framework.Services.TimeServices
{
    public interface ITimerListener
    {
        void OnTimeElapsed(TimerToken token);
    }
}