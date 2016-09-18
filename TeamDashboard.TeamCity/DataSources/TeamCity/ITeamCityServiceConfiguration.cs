using NoeticTools.TeamStatusBoard.Framework.Config;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity
{
    public interface ITeamCityDataSourceConfiguration : IItemConfiguration
    {
        string Url { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string AgentsFilter { get; set; }
    }
}