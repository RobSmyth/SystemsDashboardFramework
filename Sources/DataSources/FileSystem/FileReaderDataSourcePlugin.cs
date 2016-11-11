using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.FileSystem
{
    public class FileReaderDataSourcePlugin : IPlugin
    {
        public int Rank => 80;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("FileReader");
            var dataService = new FileReaderDataService(services, dataSource);
            services.Register(dataService);
            services.DataService.Register(dataService.Name, dataSource);

            // todo - user should be able to add any number of FileReader datasources
        }
    }
}