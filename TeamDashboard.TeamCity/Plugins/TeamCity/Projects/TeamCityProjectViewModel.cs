using System;
using System.Linq;
using NoeticTools.TeamStatusBoard.Framework;
using NoeticTools.TeamStatusBoard.Framework.DataSources.Jira;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Projects
{
    public class TeamCityProjectViewModel : NotifyingViewModelBase, IProject
    {
        private readonly ITimeCachedArray<Project> _projectCache;

        public TeamCityProjectViewModel(string name, ITimeCachedArray<Project> projectCache)
        {
            _projectCache = projectCache;
            Name = name;
        }

        public string Name { get; }
        public Project Inner { get { return _projectCache.Items.SingleOrDefault(x => Name.Equals(x.Name, StringComparison.InvariantCultureIgnoreCase)); } }
    }
}