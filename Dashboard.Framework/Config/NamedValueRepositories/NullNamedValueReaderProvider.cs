

namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public class NullNamedValueRepositoryProvider : INamedValueReaderProvider
    {
        public bool CanHandle(string name)
        {
            return true;
        }

        public INamedValueRepository Get(string name)
        {
            return new NullValueRepository();
        }
    }
}