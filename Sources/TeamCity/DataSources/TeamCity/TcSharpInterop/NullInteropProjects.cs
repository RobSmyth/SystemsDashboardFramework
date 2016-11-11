using System.Collections.Generic;
using TeamCitySharp.ActionTypes;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.DataSource.TeamCity.DataSources.TeamCity.TcSharpInterop
{
    public sealed class NullInteropProjects : IProjects
    {
        public List<Project> All()
        {
            return new List<Project>();
        }

        public Project ByName(string projectLocatorName)
        {
            return new NullInteropProject(projectLocatorName);
        }

        public Project ById(string projectLocatorId)
        {
            return new NullInteropProject("Unknown");
        }

        public Project Details(Project project)
        {
            return project;
        }

        public Project Create(string projectName)
        {
            return new NullInteropProject(projectName);
        }

        public void Delete(string projectName)
        {
        }

        public void DeleteProjectParameter(string projectName, string parameterName)
        {
        }

        public void SetProjectParameter(string projectName, string settingName, string settingValue)
        {
        }
    }
}