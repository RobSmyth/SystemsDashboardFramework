namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public interface INamedValueReaderProvider
    {
        bool CanHandle(string name);
        INamedValueReader Get(string name);
    }
}