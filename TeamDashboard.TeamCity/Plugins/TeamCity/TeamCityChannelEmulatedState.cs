using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using NoeticTools.TeamStatusBoard.Framework.Services.DataServices;
using NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity.Agents;
using TeamCitySharp.DomainEntities;


namespace NoeticTools.TeamStatusBoard.TeamCity.Plugins.TeamCity
{
    internal class TeamCityChannelEmulatedState : ITeamCityChannelState
    {
        private readonly Build _nullBuild = new NullBuild();
        private readonly Dictionary<string, BuildConfig> _buildConfigurations = new Dictionary<string, BuildConfig>();
        private readonly Dictionary<string, Project> _projects = new Dictionary<string, Project>();
        private readonly Random _rand;
        private readonly object _syncRoot = new object();
        private readonly string[] _status = {"SUCCESS", "SUCCESS", "SUCCESS", "SUCCESS", "FAILURE", "UNKNOWN"};
        private ILog _logger;

        public TeamCityChannelEmulatedState(IDataSource repository)
        {
            _rand = new Random(DateTime.Now.Millisecond);
            _logger = LogManager.GetLogger("DateSources.TeamCity.Emulated");

            repository.Write("Projects.Count", 0);
            foreach (var projectName in ProjectNames)
            {
                repository.Write($"Project.{projectName}.Status", "Emulated");
            }
            repository.Write("Projects.Count", ProjectNames.Length);
        }

        public string[] ProjectNames => new[] {"Project A", "Project B"};
        public bool IsConnected => true;

        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public async Task<Build> GetLastBuild(string projectName, string buildConfigurationName)
        {
            var randomValue = _rand.Next(0, _status.Length - 1);
            return await Task.Run(() =>
            {
                var project = GetProject(projectName);
                var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
                return CreateBuild(buildConfiguration, randomValue);
            });
        }

        public async Task<Build> GetLastSuccessfulBuild(string projectName, string buildConfigurationName)
        {
            var randomValue = _rand.Next(0, _status.Length - 1);
            return await Task.Run(() =>
            {
                var project = GetProject(projectName);
                var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
                var build = CreateBuild(buildConfiguration, randomValue);
                build.Status = _rand.Next(0, 5) <= 1 ? "FAILURE" : "SUCCESS";
                return build;
            });
        }

        public async Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName)
        {
            if (_rand.Next(1, 10) <= 4)
            {
                return new Build[0];
            }

            var randomValue = _rand.Next(0, _status.Length - 1);
            return await Task.Run(() =>
            {
                var project = GetProject(projectName);
                var buildConfiguration = GetBuildConfiguration(project, buildConfigurationName);
                var build = CreateBuild(buildConfiguration, randomValue);
                build.Status = randomValue <= 1 ? "RUNNING" : "RUNNING FAILED";
                return new[] {build};
            });
        }

        public Task<string[]> GetConfigurationNames(string projectName)
        {
            return Task.Run(() => new[] {"Configuration 1", "Configuration 2", "Configuration 3"});
        }

        public Task<IBuildAgent[]> GetAgents()
        {
            return Task.Run(() => new IBuildAgent[0]);
        }

        public Task<IBuildAgent> GetAgent(string name)
        {
            return Task.Run<IBuildAgent>(() => new NullBuildAgent(name));
        }

        void ITeamCityChannelState.Leave()
        {
        }

        void ITeamCityChannelState.Enter()
        {
        }

        public async Task<Build[]> GetRunningBuilds(string projectName, string buildConfigurationName, string branchName)
        {
            return await GetRunningBuilds(projectName, buildConfigurationName);
        }

        private Build CreateBuild(BuildConfig buildConfiguration, int randomValue)
        {
            return new Build {Status = GetBuildState(randomValue), BuildConfig = buildConfiguration, Number = "1.2.3-1234"};
        }

        private string GetBuildState(int randomValue)
        {
            return _status[randomValue];
        }

        private Project GetProject(string name)
        {
            lock (_syncRoot)
            {
                if (!_projects.ContainsKey(name))
                {
                    _projects.Add(name, new Project {Name = name});
                }
                return _projects[name];
            }
        }

        private BuildConfig GetBuildConfiguration(Project project, string name)
        {
            lock (_syncRoot)
            {
                if (!_buildConfigurations.ContainsKey(name))
                {
                    _buildConfigurations.Add(name, new BuildConfig {Name = name, Project = project});
                }
                return _buildConfigurations[name];
            }
        }
    }
}