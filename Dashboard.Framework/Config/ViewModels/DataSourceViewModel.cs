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
            Properties = new ObservableCollection<IDataSourcePropertyViewModel>(_dataSource.GetAllNames().Select(x => new DataSourcePropertyViewModel(_dataSource, x)));
            _dataSource.AddListener(this);
        }

        public string TypeName
        {
            get { return _dataSource.TypeName; }
        }

        public string Name
        {
            get { return _dataSource.Name; }
        }

        public ICommand ConfigureCommand { get; }

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