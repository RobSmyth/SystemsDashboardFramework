using NoeticTools.TeamStatusBoard.Common;


namespace NoeticTools.TeamStatusBoard.Framework.Styles
{
    public interface IStatusBoardStyle
    {
        string StyleUrl { get; set; }
        IEventBroadcaster Broadcaster { get; }
    }
}