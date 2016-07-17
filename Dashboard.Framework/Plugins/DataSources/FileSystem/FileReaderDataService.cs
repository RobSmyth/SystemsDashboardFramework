using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.FileSystem
{
    public class FileReaderDataService : IService
    {
        public FileReaderDataService(IServices services, IDataSource innerDataSource)
        {
            innerDataSource.Write("Service.Name", "FileReader");
            innerDataSource.Write("Service.Mode", "Run");
            innerDataSource.Write("Service.Status", "Stopped");

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