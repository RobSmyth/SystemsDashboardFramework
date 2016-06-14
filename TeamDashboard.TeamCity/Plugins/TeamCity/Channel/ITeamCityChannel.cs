using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Channel
{
    public interface ITeamCityChannel : ITeamCityIoChannel
    {
        void Stop();
        void Start();
        void Configure();
        IProjectRepository Projects { get; }
    }
}