using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations;
using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Projects;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TestData
{
    public sealed class TestDataBuildConfigurationFactory : IBuildConfigurationRepositoryFactory
    {
        public IBuildConfigurationRepository Create(IProject project)
        {
            return new TestDataBuildConfigurationRepository();
        }
    }
}