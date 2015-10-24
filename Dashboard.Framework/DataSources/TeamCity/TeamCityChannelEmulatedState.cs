using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.TeamCity;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace NoeticTools.Dashboard.Framework.DataSources.TeamCity
{
    internal class TeamCityChannelEmulatedState : ITeamCityChannel
    {
        private readonly TeamCityClient _client;
        private readonly Dictionary<string, BuildConfig> _buildConfigurations = new Dictionary<string, BuildConfig>();
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        private Random _rand;

        public TeamCityChannelEmulatedState(TeamCityClient client, IStateEngine<ITeamCityChannel> stateEngine)
        {
            _client = client;
            _rand = new Random(DateTime.Now.Millisecond);
        }

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public Build GetLastBuild(string projectName, string buildConfigurationName)
        {
            var project = GetProject(projectName);
            var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
            return CreateBuild(buildConfiguration);
        }

        private Build CreateBuild(BuildConfig buildConfiguration)
        {
            return new Build() {Status = GetBuildState(), BuildConfig = buildConfiguration, Number = "1.2.3-1234"};
        }

        private string GetBuildState()
        {
            var status = new[] { "SUCCESS", "SUCCESS", "SUCCESS", "SUCCESS", "FAILURE", "UNKNOWN" };
            return status[_rand.Next(0, status.Length - 1)];
        }

        private Project GetProject(string name)
        {
            if (!_projects.ContainsKey(name))
            {
                _projects.Add(name, new Project() {Name = name});
            }
            return _projects[name];
        }

        private BuildConfig GetBuildConfiguration(Project project, string name)
        {
            if (!_buildConfigurations.ContainsKey(name))
            {
                _buildConfigurations.Add(name, new BuildConfig() {Name = name, Project = project});
            }
            return _buildConfigurations[name];
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            var project = GetProject(projectName);
            var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
            return CreateBuild(buildConfiguration); 
        }

        public Build GetLastSuccessfulBuild(string projectName, string buildConfigurationName, string branchName)
        {
            var project = GetProject(projectName);
            var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
            return CreateBuild(buildConfiguration);
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName)
        {
            if (_rand.Next(1, 10) <= 3)
            {
                return null;
            }

            var project = GetProject(projectName);
            var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
            var build = CreateBuild(buildConfiguration);
            build.Status = _rand.Next(1, 10) <= 1 ? "FAILURE" : "SUCCESS";
            return build;
        }

        public Build GetRunningBuild(string projectName, string buildConfigurationName, string branchName)
        {
            return GetRunningBuild(projectName, buildConfigurationName);
        }
    }
}