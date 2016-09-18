using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel
    {
        private readonly IDataSource _dataSource;

        public DataSourceViewModel(IDataSource dataSource)
        {
            _dataSource = dataSource;
            Properties = new ObservableCollection<IDataSourcePropertyViewModel>(_dataSource.GetAllNames().Select(x => new DataSourcePropertyViewModel(_dataSource, x)));
        }

        public string TypeName => _dataSource.TypeName;

        public ICommand ConfigureCommand { get; }

        public ObservableCollection<IDataSourcePropertyViewModel> Properties { get; }
    }
}