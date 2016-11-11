using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TestDataSource
{
    public sealed class TestDataBuildConfigurationFactory : IBuildConfigurationRepositoryFactory
    {
        public IBuildConfigurationRepository Create(IProject project)
        {
            return new TestDataBuildConfigurationRepository();
        }
    }
}