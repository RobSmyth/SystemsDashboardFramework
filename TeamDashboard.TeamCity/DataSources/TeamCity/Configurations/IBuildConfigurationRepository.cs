namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations
{
    public interface IBuildConfigurationRepository
    {
        IBuildConfiguration[] GetAll();
    }
}