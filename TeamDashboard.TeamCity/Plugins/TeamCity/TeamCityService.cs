using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public sealed class TeamCityService : ITeamCityService
    {
        public string Name => "TeamCity";

        public TeamCityService(ITeamCityChannel channel)
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