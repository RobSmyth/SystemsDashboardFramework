namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public interface IDataSourcePropertyViewModel
    {
        string Name { get; }
        object Value { get; }
        bool IsReadOnly { get; }
    }
}