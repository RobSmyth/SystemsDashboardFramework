using System.Linq;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Config
{
    public class DataSourceNamedValueReaderProvider : INamedValueReaderProvider
    {
        private readonly IServices _services;

        public DataSourceNamedValueReaderProvider(IServices services)
        {
            _services = services;
        }

        public bool CanHandle(string name)
        {
            return name.StartsWith("=");
        }

        public INamedValueReader Get(string name)
        {
            var dataService = _services.DataService.GetAllDataSources().FirstOrDefault(x => name.Substring(1).StartsWith(x.TypeName));
            if (dataService != null)
            {
                return new DataSourceNamedValueReader(dataService);
            }
            return new NullValueReader();
        }
    }
}