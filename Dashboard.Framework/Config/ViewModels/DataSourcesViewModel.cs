using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NoeticTools.TeamStatusBoard.Common.ViewModels;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourcesViewModel : NotifyingViewModelBase
    {
        public DataSourcesViewModel(IServices services)
        {
            var allDataSources = services.DataService.GetAllDataSources();
            DataSources = new BindingList<IDataSourceViewModel>(allDataSources.Select(x => new DataSourceViewModel(x)).Cast<IDataSourceViewModel>().ToList());
            DataSources.AllowNew = true;
            DataSources.AddingNew += (sender, args) => args.NewObject = new NewDataSourceViewModel() {TypeName = AvailableDataSourceTypes.Last(), Name = ""};
            AvailableDataSourceTypes = allDataSources.Select(x => x.TypeName).Distinct().ToArray();
        }

        public IEnumerable<string> AvailableDataSourceTypes { get; private set; }

        public BindingList<IDataSourceViewModel> DataSources { get; private set; }
    }
}