namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations
{
    public interface IBuildConfigurationRepository
    {
        IBuildConfiguration[] GetAll();
    }
}