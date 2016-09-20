using NoeticTools.TeamStatusBoard.DataSources.FileSystem;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.DataSources.TestData
{
    public class TestDataSourcePlugin : IPlugin
    {
        public int Rank => 100;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("TestData");
            var dataService = new TestDataService(services, dataSource);
            services.Register(dataService);
            services.DataService.Register(dataService.Name, dataSource);
        }
    }
}