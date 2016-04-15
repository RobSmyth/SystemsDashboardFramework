using System.Collections.Generic;
using System.Linq;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel
    {
        private readonly IDataSource _dataSource;

        public DataSourceViewModel(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public string Name => _dataSource.Name;

        public IEnumerable<IDataSourcePropertyViewModel> Properties => _dataSource.GetAllNames().Select(x => new DataSourcePropertyViewModel(_dataSource, x));
    }
}