using System;
using System.Collections.ObjectModel;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel, IDataChangeListener
    {
        private readonly IDataSource _dataSource;

        public DataSourceViewModel(IDataSource dataSource)
        {
            _dataSource = dataSource;
            Properties = new ObservableCollection<IDataSourcePropertyViewModel>(_dataSource.GetAllNames().Select(x => new DataSourcePropertyViewModel(_dataSource, x)));
            _dataSource.AddListener(this);
        }

        public string Name => _dataSource.Name;

        public ObservableCollection<IDataSourcePropertyViewModel> Properties { get; }

        void IDataChangeListener.OnChanged()
        {
            var propertyNames = _dataSource.GetAllNames();
            foreach (var name in propertyNames)
            {
                if (!Properties.Any(x => x.Name.Equals(name, StringComparison.InvariantCulture)))
                {
                    Properties.Add(new DataSourcePropertyViewModel(_dataSource, name));
                }
            }
        }
    }
}