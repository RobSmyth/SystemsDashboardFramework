using NoeticTools.SystemsDashboard.Framework.DataSources.TeamCity;
using NoeticTools.SystemsDashboard.Framework.Plugins;
using NoeticTools.SystemsDashboard.Framework.Services;
using NoeticTools.TeamStatusBoard.Framework.Plugins;
using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.DataSources.TeamCity
{
    public class TeamCityServicePlugin : IPlugin
    {
        public int Rank => 90;

        public void Register(IServices services)
        {
            services.Register(new TeamCityService(services, new BuildAgentRepository()));
        }
    }
}