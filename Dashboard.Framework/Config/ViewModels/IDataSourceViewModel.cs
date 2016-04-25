using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public interface IDataSourceViewModel
    {
        string TypeName { get; }
        string Name { get; }
        ICommand ConfigureCommand { get; }
    }
}