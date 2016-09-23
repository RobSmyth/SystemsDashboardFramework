namespace NoeticTools.TeamStatusBoard.Framework
{
    public interface IStatusBoardStyle
    {
        string StyleUrl { get; set; }
        IEventBroadcaster Broadcaster { get; }
    }
}