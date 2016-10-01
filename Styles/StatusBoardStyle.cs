using NoeticTools.TeamStatusBoard.Common;


namespace NoeticTools.TeamStatusBoard.Framework.Styles
{
    public sealed class StatusBoardStyle : IStatusBoardStyle
    {
        private string _styleUrl;

        public StatusBoardStyle()
        {
            _styleUrl = "/NoeticTools.TeamStatusBoard.Styles;component/Style1.xaml";
            //_styleUrl = "/NoeticTools.TeamStatusBoard.Common;component/Style2.xaml";
            Broadcaster = new EventBroadcaster();
        }

        public string StyleUrl
        {
            get { return _styleUrl; }
            set
            {
                _styleUrl = value;
                Broadcaster.Fire();
            }
        }

        public IEventBroadcaster Broadcaster { get; }
    }
}