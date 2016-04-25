using System.Collections.Generic;


namespace NoeticTools.TeamStatusBoard.Framework.DataSources
{
    public class DataPropertiesRepository : IPropertiesDataService
    {
        private readonly IDataPropertyViewModelFactory _factory;
        private readonly IDictionary<string, IDataPropertyViewModel> _properties = new Dictionary<string, IDataPropertyViewModel>();

        public DataPropertiesRepository(IDataPropertyViewModelFactory factory)
        {
            _factory = factory;
        }

        public IDataPropertyViewModel<T> Add<T>(string providerName, string propertyname, T value)
        {
            var fullName = $"{providerName}.{propertyname}";
            var propertyViewModel = _factory.Create<T>(fullName);
            _properties.Add(fullName, propertyViewModel);
            return propertyViewModel;
        }

        public bool Has<T>(string propertyname)
        {
            return _properties.ContainsKey(propertyname);
        }

        public IDataPropertyViewModel<T> Get<T>(string propertyName)
        {
            if (!Has<T>(propertyName))
            {
                var nameParts = propertyName.Split('.');
                if (nameParts.Length < 2)
                {
                    return new NullDataPropertyViewModel<T>();
                }
                var providerName = nameParts[0];
                return Add(providerName, propertyName.Remove(0, providerName.Length + 1), default(T));
            }
            return (IDataPropertyViewModel<T>) _properties[propertyName];
        }

        public IDataPropertyViewModel<T> Get<T>(string providerName, string propertyname)
        {
            return Get<T>($"{providerName}.{propertyname}");
        }
    }
}