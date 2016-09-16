using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.FileSystem
{
    public class FileReaderDataService : IService
    {
        public FileReaderDataService(IServices services, IDataSource dataSource)
        {
            dataSource.Write("Service.Name", "FileReader");
            dataSource.Write("Service.Mode", "Run");
            dataSource.Write("Service.Status", "Stopped");

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