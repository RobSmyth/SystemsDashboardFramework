using NoeticTools.TeamStatusBoard.Persistence.Xml;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity
{
    public interface ITeamCityDataSourceConfiguration : IItemConfiguration
    {
        string Url { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string AgentsFilter { get; set; }
    }
}