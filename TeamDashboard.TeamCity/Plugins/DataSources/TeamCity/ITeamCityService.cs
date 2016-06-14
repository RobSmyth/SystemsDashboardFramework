using NoeticTools.TeamStatusBoard.Framework.Services;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Channel;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity
{
    public interface ITeamCityService : IService
    {
        ITeamCityChannel Channel { get; }
    }
}