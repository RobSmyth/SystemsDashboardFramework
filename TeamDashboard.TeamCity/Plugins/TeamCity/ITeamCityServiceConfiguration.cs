using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    public interface ITeamCityServiceConfiguration : IItemConfiguration
    {
        string Url { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}