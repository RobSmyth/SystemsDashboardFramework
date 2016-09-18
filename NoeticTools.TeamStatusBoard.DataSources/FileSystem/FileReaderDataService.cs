using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.FileSystem
{
    public class FileReaderDataService : IService
    {
        private const string PropertyTag = "FileReader";

        public FileReaderDataService(IServices services, IDataSource dataSource)
        {
            dataSource.Set("Service.Name", "FileReader", PropertiesFlags.ReadOnly, PropertyTag);
            dataSource.Set("Service.Mode", "Run", PropertiesFlags.ReadOnly, PropertyTag);
            dataSource.Set("Service.Status", "Stopped", PropertiesFlags.ReadOnly, PropertyTag);

            // todo - ability to configure a service from services view
        }

        public string Name => "FileReader";

        public void Stop()
        {
        }

        public void Start()
        {
        }
    }
}