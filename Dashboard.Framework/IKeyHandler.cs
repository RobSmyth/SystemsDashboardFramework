using System.Windows.Input;


namespace NoeticTools.Dashboard.Framework
{
    public interface IKeyHandler
    {
        bool CanHandle(Key key);
        void Handle(Key key);
    }
}