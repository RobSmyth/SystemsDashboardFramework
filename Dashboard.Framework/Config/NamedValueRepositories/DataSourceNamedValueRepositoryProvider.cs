using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config.NamedValueRepositories
{
    public class DataSourceNamedValueRepositoryProvider : INamedValueReaderProvider
    {
        private readonly IServices _services;

        public DataSourceNamedValueRepositoryProvider(IServices services)
        {
            _services = services;
        }

        public bool CanHandle(string name)
        {
            return name.StartsWith("=");
        }

        public INamedValueRepository Get(string name)
        {
            var dataService = _services.DataService.GetAllDataSources().FirstOrDefault(x => name.Substring(1).StartsWith(x.TypeName));
            if (dataService != null)
            {
                return new DataSourceNamedValueRepository(dataService);
            }
            return new NullValueRepository();
        }
    }
}