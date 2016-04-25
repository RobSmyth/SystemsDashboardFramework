using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Input
{
    public interface IKeyHandler
    {
        bool CanHandle(Key key);
        void Handle(Key key);
    }
}