namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public interface INamedValueReaderProvider
    {
        bool CanHandle(string name);
        INamedValueRepository Get(string name);
    }
}