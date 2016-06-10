namespace NoeticTools.TeamStatusBoard.Framework.DataSources.Jira
{
    public interface ITimeCachedArray<T>
    {
        T[] Items { get; }
    }
}