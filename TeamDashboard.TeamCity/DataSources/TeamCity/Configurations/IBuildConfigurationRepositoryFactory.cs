using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations
{
    public interface IBuildConfigurationRepositoryFactory
    {
        IBuildConfigurationRepository Create(IProject project);
    }
}