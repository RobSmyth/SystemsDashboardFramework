using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins.DataSources.TeamCity
{
    public class TeamCityServicePlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("TeamCity", 1);
            services.Register(new TeamCityService(services, new BuildAgentRepository(dataSource), dataSource));
            services.DataService.Register("TeamCity", dataSource);
        }
    }
}