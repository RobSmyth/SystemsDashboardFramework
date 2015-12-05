using System.Windows.Input;


namespace NoeticTools.SystemsDashboard.Framework
{
    public interface IKeyHandler
    {
        bool CanHandle(Key key);
        void Handle(Key key);
    }
}