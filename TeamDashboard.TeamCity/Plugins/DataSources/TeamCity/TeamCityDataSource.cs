using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity
{
    public sealed class TeamCityDataSource : ITeamCityService
    {
        public string Name => "TeamCity";

        public TeamCityDataSource(ITeamCityChannel channel, IProjectRepository projectRepository)
        {
            Channel = channel;
            Projects = projectRepository;
        }

        public ITeamCityChannel Channel { get; }
        public IProjectRepository Projects { get; set; }

        public void Stop()
        {
            Channel.Stop();
        }

        public void Start()
        {
            Channel.Start();
        }
    }
}