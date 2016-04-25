using System.Windows.Input;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class NewDataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel
    {
        public string TypeName { get; set; }

        public string Name { get; set; }

        public ICommand ConfigureCommand { get; }
    }
}