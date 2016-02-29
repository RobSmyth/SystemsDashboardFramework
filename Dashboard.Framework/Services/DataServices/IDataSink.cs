namespace NoeticTools.SystemsDashboard.Framework.Services.DataServices
{
    public interface IDataSink
    {
        string Name { get; }
        string ShortName { get; }
        void AddListener(IDataChangeListener listener);
    }
}