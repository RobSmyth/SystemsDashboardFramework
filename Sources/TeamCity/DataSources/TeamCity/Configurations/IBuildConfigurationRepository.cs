namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Configurations
{
    public interface IBuildConfigurationRepository
    {
        IBuildConfiguration[] GetAll();
        IBuildConfiguration Get(string name);
    }
}