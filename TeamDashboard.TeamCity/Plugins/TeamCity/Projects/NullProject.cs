using System;
using System.Threading.Tasks;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public sealed class NullProject : IProject
    {
        public bool Archived { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public BuildTypeWrapper BuildTypes { get; set; }
        public Parameters Parameters { get; set; }
        public async Task<Build[]> GetRunningBuilds(string buildConfigurationName)
        {
            return new Build[0];
        }
    }
}