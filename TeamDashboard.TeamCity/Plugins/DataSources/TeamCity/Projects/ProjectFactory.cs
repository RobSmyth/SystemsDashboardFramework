using NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Configurations;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.DataSources.TeamCity.Projects
{
    public sealed class ProjectFactory : IProjectFactory
    {
        private readonly BuildConfigurationRepositoryFactory _buildConfigurationRepositoryFactory;

        public ProjectFactory(BuildConfigurationRepositoryFactory buildConfigurationRepositoryFactory)
        {
            _buildConfigurationRepositoryFactory = buildConfigurationRepositoryFactory;
        }

        public IProject Create(TeamCitySharp.DomainEntities.Project inner)
        {
            return new Project(inner, _buildConfigurationRepositoryFactory);
        }
    }
}