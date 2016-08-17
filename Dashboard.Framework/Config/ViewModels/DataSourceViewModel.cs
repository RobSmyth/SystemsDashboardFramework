using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourceViewModel : NotifyingViewModelBase, IDataSourceViewModel, IDataChangeListener
    {
        private readonly IDataSource _dataSource;

        public DataSourceViewModel(IDataSource dataSource)
        {
            _dataSource = dataSource;
            Properties = new ObservableCollection<IDataSourcePropertyViewModel>();
            UpdateProperties();
            _dataSource.AddListener(this);
        }

        public string TypeName => _dataSource.TypeName;

        public string Name => _dataSource.Name;

        public ICommand ConfigureCommand { get; }

        public ObservableCollection<IDataSourcePropertyViewModel> Properties { get; }

        void IDataChangeListener.OnChanged()
        {
            UpdateProperties();
        }

        private void UpdateProperties()
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