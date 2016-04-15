using System.Collections.Generic;
using System.Linq;
using NoeticTools.SystemsDashboard.Framework;
using NoeticTools.SystemsDashboard.Framework.Dashboards;
using NoeticTools.SystemsDashboard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourcesViewModel : NotifyingViewModelBase
    {

        public DataSourcesViewModel(IServices services)
        {
            DataSources = services.DataService.GetAllDataSources().Select(x => new DataSourceViewModel(x));
        }

        public IEnumerable<IDataSourceViewModel> DataSources { get; private set; }
    }
}