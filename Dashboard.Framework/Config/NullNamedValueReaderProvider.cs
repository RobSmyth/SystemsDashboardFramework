namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public class NullNamedValueReaderProvider : INamedValueReaderProvider
    {
        public bool CanHandle(string name)
        {
            return true;
        }

        public INamedValueReader Get(string name)
        {
            return new NullValueReader();
        }
    }
}