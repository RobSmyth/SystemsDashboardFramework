using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Config.ViewModels
{
    public sealed class DataSourcePropertyViewModel : NotifyingViewModelBase, IDataSourcePropertyViewModel
    {
        private readonly IDataSource _dataSource;

        public DataSourcePropertyViewModel(IDataSource dataSource, string propertyName)
        {
            _dataSource = dataSource;
            Name = propertyName;
        }

        public string Name { get; }

        public object Value
        {
            get { return _dataSource.Read<object>(Name); }
            set {  _dataSource.Write<object>(Name, value);}
        }

        public bool IsReadOnly
        {
            get { return _dataSource.IsReadOnly(Name); }
        }
    }
}