namespace NoeticTools.TeamStatusBoard.Framework.DataSources
{
    public interface IDataPropertyViewModelFactory
    {
        IDataPropertyViewModel<T> Create<T>(string fullName);
    }
}