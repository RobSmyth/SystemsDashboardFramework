using NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.FileSystem
{
    public class FileReaderDataSourcePlugin : IPlugin
    {
        public int Rank => 80;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("FileReader", 1);
            services.Register(new FileReaderService(services, dataSource));
            services.DataService.Register("FileReader", dataSource);
        }
    }
}