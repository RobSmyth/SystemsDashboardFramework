using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Common.ViewModels;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class NewDataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel
    {
        public string TypeName { get; set; }

        public string Name { get; set; }

        public ICommand ConfigureCommand { get; }
    }
}