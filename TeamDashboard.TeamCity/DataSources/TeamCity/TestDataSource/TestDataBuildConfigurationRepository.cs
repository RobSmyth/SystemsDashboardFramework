using NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.Configurations;


namespace NoeticTools.TeamStatusBoard.TeamCity.DataSources.TeamCity.TestDataSource
{
    public class TestDataBuildConfigurationRepository : IBuildConfigurationRepository
    {
        public IBuildConfiguration[] GetAll()
        {
            return new IBuildConfiguration[0];
        }

        public IBuildConfiguration Get(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}