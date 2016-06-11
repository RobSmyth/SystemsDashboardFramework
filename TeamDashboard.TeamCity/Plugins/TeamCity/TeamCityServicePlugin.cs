using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.TcSharpInterop;
using TeamCitySharp;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public class TeamCityServicePlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            var dataSource = new DataRepositoryFactory().Create("TeamCity", "0");
            var configuration = new TeamCityServiceConfiguration(services.Configuration.Services.GetService("TeamCity"));
            services.Register(new TeamCityService(services, 
                new TcSharpTeamCityClient(new TeamCityClient(configuration.Url)), new BuildAgentRepository(dataSource), dataSource, configuration));
            services.DataService.Register("TeamCity", dataSource, new NullTileControllerProvider());
        }
    }
}