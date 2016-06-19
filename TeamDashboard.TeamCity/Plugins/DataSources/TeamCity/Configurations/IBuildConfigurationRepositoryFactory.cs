using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations
{
    public interface IBuildConfigurationRepositoryFactory
    {
        IBuildConfigurationRepository Create(IProject project);
    }
}