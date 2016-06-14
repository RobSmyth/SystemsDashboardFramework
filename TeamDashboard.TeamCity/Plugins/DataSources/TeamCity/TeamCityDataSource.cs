using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity
{
    public sealed class TeamCityDataSource : ITeamCityService
    {
        public string Name => "TeamCity";

        public TeamCityDataSource(ITeamCityChannel channel)
        {
            Channel = channel;
        }

        public ITeamCityChannel Channel { get; }

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